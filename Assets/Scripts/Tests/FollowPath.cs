using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public List<Vector3> Waypoints;
    public List<Vector3> OffsetWaypoints;

    [HideInInspector]
    public uint CurrentWaypointIndex;
    [HideInInspector]
    public uint PreviousWaypointIndex;

    public Mode FollowMode;
    public float FollowForce;
    public float maxVelocity;
    public float Radius;

    public bool Breakable;
    public float BreakThreshold;
    [HideInInspector]
    public bool IsBroken;

    private Rigidbody _Rigidbody;
    private Vector3 _FollowDirection;
    private Vector3 _ClosestPointOnPath;
    private Vector3 _ClosestNextPointOnPath;
    private bool _IsGoingForward;

    public enum Mode
    {
        Loop,
        BackAndForth,
        Single
    }

    private void Start()
    {
        if (OffsetWaypoints == null)
            Debug.LogError("No waypoints in path, or OffsetWaypoints not set");
        if (Waypoints.Count < 2)
            Debug.LogError("At least two waypoints are needed to follow a path");
        if (!TryGetComponent(out _Rigidbody))
            Debug.LogError("Failed to get RigidBody component in FollowPath script");

        IsBroken = false;
        _IsGoingForward = false;

        if (!TryGetComponent(out _Rigidbody))
            Debug.LogError("Failed to get Rigidbody component in FollowPath");
    }

    void FixedUpdate()
    {
        Vector3 position = transform.position;

        //Closest point on path
        Vector3 pathNormal = OffsetWaypoints[(int)PreviousWaypointIndex] - OffsetWaypoints[(int)CurrentWaypointIndex];
        pathNormal.Normalize();
        _ClosestPointOnPath = Vector3.Project(position - OffsetWaypoints[(int)PreviousWaypointIndex], pathNormal) + OffsetWaypoints[(int)PreviousWaypointIndex];

        _ClosestNextPointOnPath = Vector3.Project(position - pathNormal - OffsetWaypoints[(int)PreviousWaypointIndex], pathNormal) + OffsetWaypoints[(int)PreviousWaypointIndex];

        _FollowDirection = Vector3.Distance(position, OffsetWaypoints[(int)CurrentWaypointIndex]) < Vector3.Distance(_ClosestNextPointOnPath, OffsetWaypoints[(int)CurrentWaypointIndex]) ? OffsetWaypoints[(int)CurrentWaypointIndex] - position : _ClosestNextPointOnPath - position;

        //If arrived at waypoint
        if (_FollowDirection.magnitude < Radius)
        {
            PreviousWaypointIndex = CurrentWaypointIndex;
            CurrentWaypointIndex = GetNextPathIndex();
        }

        _FollowDirection.Normalize();

        if (new Vector3(_Rigidbody.velocity.x, 0, _Rigidbody.velocity.z).magnitude < maxVelocity)
            _Rigidbody.AddForce(new Vector3(_FollowDirection.x, 0, _FollowDirection.z) * FollowForce);
        _Rigidbody.velocity = Vector3.Normalize(new Vector3(_Rigidbody.velocity.x, 0, _Rigidbody.velocity.z)) * maxVelocity + new Vector3(0, _Rigidbody.velocity.y, 0);

        if (Breakable)
        {
            IsBroken = Vector3.Distance(position, _ClosestPointOnPath) > BreakThreshold;

            if (IsBroken)
            {
                enabled = false;
            }
        }
    }

    private uint GetNextPathIndex()
    {
        switch (FollowMode)
        {
            case Mode.Loop:
                return (CurrentWaypointIndex + 1) % (uint)OffsetWaypoints.Count;
            case Mode.Single:
                uint nextIndex = CurrentWaypointIndex + 1;
                if (CurrentWaypointIndex + 1 < (uint)OffsetWaypoints.Count)
                {
                    return nextIndex;
                }
                else
                {
                    enabled = false;
                    return CurrentWaypointIndex;
                }
            case Mode.BackAndForth:
                if (CurrentWaypointIndex == 0 || CurrentWaypointIndex + 1 == OffsetWaypoints.Count)
                    _IsGoingForward = !_IsGoingForward;
                return _IsGoingForward ? CurrentWaypointIndex + 1 : CurrentWaypointIndex - 1;
        }

        Debug.LogWarning("Unknown follow mode");
        return 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (OffsetWaypoints == null || OffsetWaypoints.Count == 0 || IsBroken)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLineStrip(OffsetWaypoints.ToArray(), FollowMode == Mode.Loop);

        for (uint i = 0; i < OffsetWaypoints.Count; i++)
        {
            if (i == CurrentWaypointIndex)
                Gizmos.color = Color.cyan;
            else if (i == PreviousWaypointIndex)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(OffsetWaypoints[(int)i], 0.1f);
        }

        if (_FollowDirection != Vector3.zero && _Rigidbody != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_Rigidbody.transform.position, _Rigidbody.transform.position + _FollowDirection);
        }

        if (_ClosestPointOnPath != null && _ClosestPointOnPath != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_ClosestPointOnPath, 0.1f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_ClosestNextPointOnPath, 0.1f);
        }
    }
#endif
}
