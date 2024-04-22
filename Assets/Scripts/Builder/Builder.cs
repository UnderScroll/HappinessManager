using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        public Structure Structure;
        public CellTypes CellTypes;

        private (Option<int3> pointedCell, Option<int3> cellToPlace) _pointed;

        private void Start()
        {
            Init();
        }

        void PlaceBlock(CellData data, int3 position)
        {
            Structure.Cells[position.x, position.y, position.z] = data;

            UpdateCell(position);
        }

        void RemoveBlock(int3 position)
        {
            Structure.Cells[position.x, position.y, position.z] = null;

            UpdateCell(position);
        }
        void ModifyBlockData(CellData data, int3 position)
        {
        }

        private void Update()
        {
            int2 mousePos = new int2((int)Input.mousePosition.x, (int)Input.mousePosition.y);

            _pointed = GetPointed(mousePos);
        }

        private void OnDrawGizmos()
        {
            int3 pointedCellPosition;
            if (_pointed.pointedCell.IsSome(out pointedCellPosition))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(new Vector3(pointedCellPosition.x, pointedCellPosition.y, pointedCellPosition.z) + transform.position, new Vector3(1, 1, 1));
            }
            int3 cellToPlacePosition;
            if (_pointed.cellToPlace.IsSome(out cellToPlacePosition))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(cellToPlacePosition.x, cellToPlacePosition.y, cellToPlacePosition.z) + transform.position, new Vector3(0.5f, 0.5f, 0.5f));
            }
        }

        private bool IsInBounds(int3 position)
        {
            return !(position.x < 0 || position.y < 0 || position.z < 0) 
                && position.x < Structure.Cells.GetLength(0) 
                && position.y < Structure.Cells.GetLength(1)
                && position.z < Structure.Cells.GetLength(2);
        }

        public void OnPlace(InputValue value)
        {
            int3 positionToPlace;
            if (_pointed.cellToPlace.IsSome(out positionToPlace))
                PlaceBlock((CellData)CellTypes.get()["BasicBlock"], positionToPlace);
        }

        public void OnRemove(InputValue value)
        {
            int3 positionToRemove;
            if (_pointed.pointedCell.IsSome(out positionToRemove))
                RemoveBlock(positionToRemove);
        }

        //Temp function
        void Init()
        {
            Structure = new Structure(new int3(10, 10, 10));

            _blockPreviews = new PreviewBlock[10, 10, 10];
            _previewLayer = 1 << LayerMask.NameToLayer("Blocks");

            CellType basicBlock = CellTypes.get()["BasicBlock"];

            PlaceBlock((CellData)basicBlock, new int3(0, 0, 0));
            PlaceBlock((CellData)basicBlock, new int3(7, 3, 2));
            PlaceBlock((CellData)basicBlock, new int3(7, 2, 2));
            PlaceBlock((CellData)basicBlock, new int3(3, 1, 3));
        }
    }
}

