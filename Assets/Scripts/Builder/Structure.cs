using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Builder
{
    [Serializable]
    public class Structure
    {
        public CellDataVolume Cells;
        public int3 Size {  get => Cells.Size; }

        public Structure(int3 size)
        {
            Cells = new CellDataVolume(size);
        }
    }

    [Serializable]
    public class CellDataVolume : IEnumerable<CellData>
    {
        [HideInInspector]
        [SerializeReference]
        private CellData[] _cells;
        [SerializeField]
        [HideInInspector]
        public int3 Size;

        public CellDataVolume(int3 size)
        {
            _cells = new CellData[size.x * size.y * size.z];
            Size = size;
        }

        public CellData this[int x, int y, int z]
        {
            get => _cells[x * Size.y * Size.z + y * Size.z + z];
            set => _cells[x * Size.y * Size.z + y * Size.z + z] = value;
        }

        public CellData this[uint x, uint y, uint z]
        {
            get => _cells[(int)(x * Size.y * Size.z + y * Size.z + z)];
            set => _cells[(int)(x * Size.y * Size.z + y * Size.z + z)] = value;
        }

        IEnumerator IEnumerable.GetEnumerator() => _cells.GetEnumerator();

        public int GetLength(int dim)
        {
            return dim switch
            {
                0 => Size.x,
                1 => Size.y,
                2 => Size.z,
                _ => throw new AccessViolationException($"Tried to acces dimention {dim} of a 3D Array"),
            };
        }

        public IEnumerator<CellData> GetEnumerator()
        {
            return ((IEnumerable<CellData>)_cells).GetEnumerator();
        }
    }
}
