using System;
using UnityEngine;
using Unity.Mathematics;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        [HideInInspector]
        public Transform StructureOrigin { set { _structureOrigin = value; } }
        private Transform _structureOrigin;

        private PreviewBlock[,,] _previewBlocks;
        private int _mainBlockLayer;
        private int _subBlocksLayer;

        private void Awake()
        {
            _mainBlockLayer = 1 << LayerMask.NameToLayer("MainBlocks");
            _subBlocksLayer = 1 << LayerMask.NameToLayer("SubBlocks");
        }

        void InitializePreviewer()
        {
            _previewBlocks = new PreviewBlock[Structure.Size.x, Structure.Size.y, Structure.Size.z];

            foreach (CellData cell in Structure.Cells)
            {
                if (cell == null)
                    continue;

                UpdateCell(cell.Position);
            }
        }

        public void ResetPreviewer()
        {
            if (_previewBlocks == null)
            {
                Debug.LogWarning("_previewBlocks was null during reset");
                return;
            }

            foreach (PreviewBlock previewBlock in _previewBlocks)
            {
                if (previewBlock == null)
                    continue;

                Destroy(previewBlock.gameObject);
            }
        }

        public void UpdateCell(int3 position)
        {
            if (_previewBlocks[position.x, position.y, position.z] != null)
                Destroy(_previewBlocks[position.x, position.y, position.z].gameObject);

            if (Structure.Cells[position.x, position.y, position.z] == null)
                return;

            CellType cellType = GetCellType(position);

            PreviewBlock previewBlock = Instantiate(cellType.PreviewCollider, _structureOrigin);
            previewBlock.Position = position;
            previewBlock.transform.Translate(position.x, position.y, position.z);

            Instantiate(cellType.Block, previewBlock.transform);

            _previewBlocks[position.x, position.y, position.z] = previewBlock;
        }

        private CellType GetCellType(int3 position)
        {
            return CellTypes.Get(Structure.Cells[position.x, position.y, position.z].Type.Name);
        }

        //(Position of cell pointed, Position of cell to place)
        public (Option<int3>, Option<int3>) GetPointed()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Option<int3> mainBlock = Option<int3>.None;
            Option<int3> mainBlockNormal = Option<int3>.None;
            PreviewBlock previewBlock = null;

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _mainBlockLayer))
            {
                previewBlock = hit.collider.gameObject.GetComponentInParent<PreviewBlock>();
                mainBlock = Option<int3>.Some(previewBlock.Position);
                mainBlockNormal = Option<int3>.Some(new int3(Convert.ToInt32(hit.normal.x), Convert.ToInt32(hit.normal.y), Convert.ToInt32(hit.normal.z)));
            }

            Option<int3> subBlock = Option<int3>.None;
            if (mainBlockNormal.IsSome(out int3 mainBlockNormalVec) && mainBlock.IsSome(out int3 mainBlockPosition))
            {
                int3 newPosition = mainBlockPosition + mainBlockNormalVec;
                if (IsInBounds(newPosition))
                    subBlock = Option<int3>.Some(newPosition);
            }
            else if (Physics.Raycast(ray, out hit, float.PositiveInfinity, _subBlocksLayer))
            {
                if (previewBlock == null) previewBlock = hit.collider.gameObject.GetComponentInParent<PreviewBlock>();
                int3 newPosition = new int3(previewBlock.Position.x, previewBlock.Position.y - 1, previewBlock.Position.z);
                if (IsInBounds(newPosition))
                    subBlock = Option<int3>.Some(newPosition);
            }

            if (subBlock.IsSome(out int3 subBlockPos) && !mainBlock.IsSome(out int3 _))
                mainBlock = Option<int3>.Some(subBlockPos + new int3(0, 1, 0));

            return (mainBlock, subBlock);
        }

        public void ActivatePreview()
        {
            foreach (PreviewBlock block in _previewBlocks)
            {
                if (block == null) continue;

                block.gameObject.SetActive(true);
            }
        }

        public void DeactivatePreview()
        {
            foreach (PreviewBlock block in _previewBlocks)
            {
                if (block == null) continue;

                block.gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            int3 pointedCellPosition;
            if (_pointed.pointedCell.IsSome(out pointedCellPosition))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(new Vector3(pointedCellPosition.x, pointedCellPosition.y, pointedCellPosition.z) + _structureOrigin.position, new Vector3(1, 1, 1));
            }
            int3 cellToPlacePosition;
            if (_pointed.cellToPlace.IsSome(out cellToPlacePosition))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(cellToPlacePosition.x, cellToPlacePosition.y, cellToPlacePosition.z) + _structureOrigin.position, new Vector3(0.7f, 0.7f, 0.7f));
            }
        }
#endif

    }
}
