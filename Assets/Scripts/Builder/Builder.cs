using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Assertions;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        public Structure Structure;
        public CellTypes CellTypes;

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
            Init();
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

        }

        private void Update()
        {
            int2 mousePos = new int2((int)Input.mousePosition.x, (int)Input.mousePosition.y);

            _pointed = GetPointed(mousePos);
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

            for (int i = 0; i < _neighborDeltaPositions.Length; i++)
            {
                int3 neighborPosition = _neighborDeltaPositions[i] + cellData.position;
                if (!IsInBounds(neighborPosition))
                    continue;

                CellData neighborCellData = Structure.Cells[neighborPosition.x, neighborPosition.y, neighborPosition.z];
                if (neighborCellData == null)
                    continue;

                ConnectionType cellConnectionType = cellData.GetConnectionType((CellData.Face)i);
                ConnectionType neighborConnectionType = cellData.GetConnectionType((CellData.Face)_inverseFaceOrder[i]);
                if (cellConnectionType.isStronger(neighborConnectionType))
                    neighborCellData.updateConnection((CellData.Face)_inverseFaceOrder[i], cellConnectionType);
                else
                    cellData.updateConnection((CellData.Face)i, neighborConnectionType);
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

                neighborCellData.updateConnection((CellData.Face)_inverseFaceOrder[i], neighborCellData.Type.DefaultConnectionType);
            }
        }

        public void OnPlace(InputValue value)
        {
            int3 positionToPlace;
            if (_pointed.cellToPlace.IsSome(out positionToPlace))
            {
                PlaceBlock((CellData)CellTypes.get()["BasicBlock"], positionToPlace);
                UpdateConnection(positionToPlace);
            }
        }

        public void OnRemove(InputValue value)
        {
            int3 positionToRemove;
            if (_pointed.pointedCell.IsSome(out positionToRemove))
            {
                RemoveBlock(positionToRemove);
                UpdateConnection(positionToRemove);
            }
        }

        //Temp function
        void Init()
        {
            Structure = new Structure(new int3(10, 10, 10));

            InitPreviewer();

            CellType basicBlock = CellTypes.get()["BasicBlock"];

            PlaceBlock((CellData)basicBlock, new int3(0, 0, 0));
            PlaceBlock((CellData)basicBlock, new int3(7, 3, 2));
            PlaceBlock((CellData)basicBlock, new int3(7, 2, 2));
            PlaceBlock((CellData)basicBlock, new int3(3, 1, 3));
        }
    }
}

