using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Builder
{
    public class CellData
    {
        public CellType Type;
        public int3 position;
        private ConnectionType[] _connections = new ConnectionType[6];

        public CellData()
        {
        }

        public CellData(CellType type)
        {
            Type = type;
            for (int i = 0; i < _connections.Length; i++)
                _connections[i] = Type.DefaultConnectionType;
        }

        public enum Face
        {
            North,
            East,
            South,
            West,
            Top,
            Bottom
        };

        public void updateConnection(Face face, ConnectionType connectionType) => _connections[(int)face] = connectionType;

        public ConnectionType GetConnectionType(Face face) => _connections[(int)face];

        public CellData Clone()
        {
            CellData clone = new CellData();
            clone.Type = Type;
            clone.position = position; 
            clone._connections = (ConnectionType[])_connections.Clone();

            return clone;
        }
    }
}

