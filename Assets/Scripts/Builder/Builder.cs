using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        public Structure Structure;
        public CellTypes CellTypes;
        public CellTypes PlacableCellTypes;
        private List<CellData> _baseBlocks;
        private CellData _selectedBlock;

        private (Option<int3> pointedCell, Option<int3> cellToPlace) _pointed;

        private readonly int3[] _neighborDeltaPositions = {
                new( 0, 0, 1),
                new( 1, 0, 0),
                new( 0, 0,-1),
                new(-1, 0, 0),
                new( 0, 1, 0),
                new( 0,-1, 0)
            };

        private readonly int[] _inverseFaceOrder = { 2, 3, 0, 1, 5, 4 };

        private void Start()
        {
            _baseBlocks = new List<CellData>();
            foreach (CellType cType in PlacableCellTypes.Get())
                _baseBlocks.Add((CellData)cType);

            Init();

            SelectBlock(0);
            if (_selectedBlock == null)
                Debug.LogError("Failed to select first block, check is CellTypes is null");
        }

        void PlaceBlock(CellData data, int3 position)
        {
            Structure.Cells[position.x, position.y, position.z] = data;
            data.position = position;

            UpdateCell(position);
        }

        void RemoveBlock(int3 position)
        {
            CellData cell = Structure.Cells[position.x, position.y, position.z];

            if (cell.Type.Removable)
            {
                Structure.Cells[position.x, position.y, position.z] = null;

                UpdateCell(position);
            }
        }
        void ModifyBlockData(CellData data, int3 position)
        {
            throw new NotImplementedException("The ModifyBlockData function is not implemented yet");
        }

        private void Update()
        {
            _pointed = GetPointed();
        }

        private bool IsInBounds(int3 position)
        {
            return position.x >= 0
                && position.y >= 0
                && position.z >= 0
                && position.x < Structure.Cells.GetLength(0)
                && position.y < Structure.Cells.GetLength(1)
                && position.z < Structure.Cells.GetLength(2);
        }

        private void UpdateConnection(CellData cellData)
        {
            Assert.IsNotNull(cellData);

            if (cellData.Type.ConnectionFaces == CellType.ConnectionFace.None)
                return;

            for (int i = 0; i < _neighborDeltaPositions.Length; i++)
            {
                if (!cellData.Type.HasConnection((CellData.Face)i))
                    continue;

                int3 neighborPosition = _neighborDeltaPositions[i] + cellData.position;
                if (!IsInBounds(neighborPosition))
                    continue;

                CellData neighborCellData = Structure.Cells[neighborPosition.x, neighborPosition.y, neighborPosition.z];
                if (neighborCellData == null)
                    continue;

                if (!neighborCellData.Type.HasConnection((CellData.Face)_inverseFaceOrder[i]))
                    continue;

                ConnectionType cellConnectionType = cellData.GetConnectionType((CellData.Face)i);
                ConnectionType neighborConnectionType = cellData.GetConnectionType((CellData.Face)_inverseFaceOrder[i]);

                if (cellConnectionType.IsStronger(neighborConnectionType))
                    neighborCellData.UpdateConnection((CellData.Face)_inverseFaceOrder[i], cellConnectionType);
                else
                    cellData.UpdateConnection((CellData.Face)i, neighborConnectionType);
            }
        }

        private void UpdateConnection(int3 position)
        {
            Assert.IsTrue(IsInBounds(position));

            CellData cellData = Structure.Cells[position.x, position.y, position.z];

            if (cellData != null)
            {
                UpdateConnection(cellData);
                return;
            }

            for (int i = 0; i < _neighborDeltaPositions.Length; i++)
            {
                int3 neighborPosition = _neighborDeltaPositions[i] + position;
                if (!IsInBounds(neighborPosition))
                    continue;

                CellData neighborCellData = Structure.Cells[neighborPosition.x, neighborPosition.y, neighborPosition.z];
                if (neighborCellData == null)
                    continue;

                neighborCellData.UpdateConnection((CellData.Face)_inverseFaceOrder[i], neighborCellData.Type.DefaultConnectionType);
            }
        }

        public void OnPlace(InputValue _)
        {
            //int3 positionToPlace;
            if (!_pointed.cellToPlace.IsSome(out int3 positionToPlace))
                return;

            if (Structure.Cells[positionToPlace.x, positionToPlace.y, positionToPlace.z] != null)
                return;

            PlaceBlock(_selectedBlock.Clone(), positionToPlace);
            UpdateConnection(positionToPlace);
            return;
        }

        public void OnRemove(InputValue value)
        {
            if (!_pointed.pointedCell.IsSome(out int3 positionToRemove))
                return;

            if (_pointed.cellToPlace.IsSome(out int3 positionToPlace)
                && (positionToPlace.y == positionToRemove.y - 1))
                    return;

            RemoveBlock(positionToRemove);
            UpdateConnection(positionToRemove);
        }

        public void OnSelectBlock1(InputValue _) => SelectBlock(0);
        public void OnSelectBlock2(InputValue _) => SelectBlock(1);
        public void OnSelectBlock3(InputValue _) => SelectBlock(2);
        public void OnSelectBlock4(InputValue _) => SelectBlock(3);

        private void SelectBlock(uint index)
        {
            if (index < _baseBlocks.Count)
                _selectedBlock = _baseBlocks[(int)index];
        }

        //FIXME: Temporary function - This should call the level loader
        void Init()
        {
            Structure = new Structure(new int3(10, 10, 10));

            InitPreviewer();

            for (uint x = 0; x < Structure.Cells.GetLength(0); x++)
                for (uint z = 0; z < Structure.Cells.GetLength(2); z++)
                    PlaceBlock((CellData)CellTypes.Get("InvisibleBlock"), new int3((int)x, 0, (int)z));

            PlaceBlock((CellData)CellTypes.Get("EmployeeBlock"), new int3(6, 8, 5));

            PlaceBlock((CellData)CellTypes.Get("EmployeeBlock"), new int3(1, 8, 4));

            SelectBlock(0);

            PlaceBlock(_selectedBlock, new int3(2, 0, 2));
        }
    }
}

