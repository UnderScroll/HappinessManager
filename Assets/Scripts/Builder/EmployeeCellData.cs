using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Builder
{
    [Serializable]
    public class EmployeeCellData : CellData
    {
        [SerializeReference]
        public MovementData Movement;

        public EmployeeCellData(CellType type)
        {
            Type = type;
            for (int i = 0; i < _connections.Length; i++)
                _connections[i] = Type.DefaultConnectionType;

            Movement = new MovementData()
            {
                HasFollowPath = false,
                Breakable = false,
                Waypoints = new(),
            };

            Movement.Waypoints.Insert(0, new Vector3(Position.x, Position.y, Position.z));
        }

        public class MovementData
        {
            //FollowPath
            public bool HasFollowPath;
            public List<Vector3> Waypoints;

            public FollowPath.Mode Mode;
            public float FollowForce;
            public float MaxVelocity;
            public float Radius;

            public bool Breakable;
            public float BreakThreshold;

            //ForceStand
            public float StandForce;
        }
    }

}
