using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEditor;
using UnityEngine;

public class EmployeeMovement : MonoBehaviour
{
    public enum FollowMode
    {
        Loop,
        BackAndForth,
        Single
    }

    public List<Vector3> waypoints;
    private uint _CurrentPointIndex;
    private Rigidbody _rigidbody;
    public double ValidationRadius;
    public uint FollowForce;
    public uint StandForce;
    public FollowMode Mode;

    private Vector3 _followDirection;
    private bool _isGoingForward;
    private bool _stoped;


    private void Awake()
    {
        if (waypoints == null)
            Debug.LogError("No waypoints in path");

        if (!TryGetComponent(out _rigidbody))
            Debug.LogError("Failed to get RigidBody component in EmployeeMovement script");

        _followDirection = Vector3.zero;

        _CurrentPointIndex = 0;
        _isGoingForward = true;
        _stoped = false;
    }

    private void FixedUpdate()
    {
        Vector3 position = _rigidbody.transform.position;

        _followDirection = waypoints[(int)_CurrentPointIndex] - position;

        if (_followDirection.magnitude < ValidationRadius)
            _CurrentPointIndex = GetNextPathIndex();

        _followDirection.y = 0;

        if (_followDirection != Vector3.zero && !_stoped)
        {
            _followDirection.Normalize();

            _rigidbody.velocity = _followDirection * FollowForce + Physics.gravity;
        }

        Quaternion rotation = _rigidbody.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
        Quaternion goToRotation = Quaternion.Lerp(rotation, targetRotation, StandForce * 0.01f * 180f / Mathf.Pow(Quaternion.Angle(rotation, targetRotation), 3));

        _rigidbody.MoveRotation(goToRotation);
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0 || _stoped)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLineStrip(waypoints.ToArray(), Mode == FollowMode.Loop);

        for (uint i = 0; i < waypoints.Count; i++)
        {
            if (i == _CurrentPointIndex)
                Gizmos.color = Color.cyan;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(waypoints[(int)i], 0.1f);
        }

        if (_followDirection == Vector3.zero || _rigidbody == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rigidbody.transform.position, _rigidbody.transform.position + _followDirection);
    }

    private uint GetNextPathIndex()
    {
        switch (Mode)
        {
            case FollowMode.Loop:
                return (_CurrentPointIndex + 1) % (uint)waypoints.Count;
            case FollowMode.Single:
                uint nextIndex = _CurrentPointIndex + 1;
                if (_CurrentPointIndex + 1 < (uint)waypoints.Count)
                {
                    return nextIndex;
                }
                else
                {
                    _stoped = true;
                    _rigidbody.velocity = Vector3.zero;
                    return _CurrentPointIndex;
                }
            case FollowMode.BackAndForth:
                if (_CurrentPointIndex == 0 || _CurrentPointIndex + 1 == waypoints.Count)
                    _isGoingForward = !_isGoingForward;
                return _isGoingForward ? _CurrentPointIndex + 1 : _CurrentPointIndex  - 1;
        }

        Debug.LogWarning("Unknown follow mode");
        return 0;
    }
}
