using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public List<Vector3> waypoints;

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

    void Awake()
    {
        if (waypoints == null)
            Debug.LogError("No waypoints in path");
        if (waypoints.Count < 2)
            Debug.LogError("At least two waypoints are needed to follow a path");
        if (!TryGetComponent(out _Rigidbody))
            Debug.LogError("Failed to get RigidBody component in FollowPath script");

        IsBroken = false;
    }

    void FixedUpdate()
    {
        Vector3 position = transform.position;

        //Closest point on path
        Vector3 pathNormal = waypoints[(int)PreviousWaypointIndex] - waypoints[(int)CurrentWaypointIndex];
        pathNormal.Normalize();
        _ClosestPointOnPath = Vector3.Project(position - waypoints[(int)PreviousWaypointIndex], pathNormal) + waypoints[(int)PreviousWaypointIndex];

        _ClosestNextPointOnPath = Vector3.Project(position - pathNormal - waypoints[(int)PreviousWaypointIndex], pathNormal) + waypoints[(int)PreviousWaypointIndex];

        _FollowDirection = Vector3.Distance(position, waypoints[(int)CurrentWaypointIndex]) < Vector3.Distance(_ClosestNextPointOnPath, waypoints[(int)CurrentWaypointIndex])  ? waypoints[(int)CurrentWaypointIndex] - position : _ClosestNextPointOnPath - position;

        //If arrived at waypoint
        if (_FollowDirection.magnitude < Radius)
        {
            PreviousWaypointIndex = CurrentWaypointIndex;
            CurrentWaypointIndex = GetNextPathIndex();
        }


        _FollowDirection.Normalize();

        if (new Vector3(_Rigidbody.velocity.x, 0, _Rigidbody.velocity.z).magnitude < maxVelocity)
            _Rigidbody.AddForce(new Vector3(_FollowDirection.x, 0, _FollowDirection.z) * FollowForce);

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
                return (CurrentWaypointIndex + 1) % (uint)waypoints.Count;
            case Mode.Single:
                uint nextIndex = CurrentWaypointIndex + 1;
                if (CurrentWaypointIndex + 1 < (uint)waypoints.Count)
                {
                    return nextIndex;
                }
                else
                {
                    enabled = false;
                    return CurrentWaypointIndex;
                }
            case Mode.BackAndForth:
                if (CurrentWaypointIndex == 0 || CurrentWaypointIndex + 1 == waypoints.Count)
                    _IsGoingForward = !_IsGoingForward;
                return _IsGoingForward ? CurrentWaypointIndex + 1 : CurrentWaypointIndex - 1;
        }

        Debug.LogWarning("Unknown follow mode");
        return 0;
    }

    private float DistanceToNextWaypoint()
    {
        return Vector3.Distance(waypoints[(int)CurrentWaypointIndex], transform.position);
    }

    private float CurrentPathLength()
    {
        return Vector3.Distance(waypoints[(int)PreviousWaypointIndex], waypoints[(int)CurrentWaypointIndex]);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0 || IsBroken)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLineStrip(waypoints.ToArray(), FollowMode == Mode.Loop);

        for (uint i = 0; i < waypoints.Count; i++)
        {
            if (i == CurrentWaypointIndex)
                Gizmos.color = Color.cyan;
            else if (i == PreviousWaypointIndex)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(waypoints[(int)i], 0.1f);
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
