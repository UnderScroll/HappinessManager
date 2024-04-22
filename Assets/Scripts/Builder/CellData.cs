using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Builder
{
public class CellData
{
        public CellType Type;
        public int3 position;
        private ConnectionType[] _connections = new ConnectionType[6];
}
}

