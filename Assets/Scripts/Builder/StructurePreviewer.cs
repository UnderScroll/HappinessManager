using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer.Internal;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        private PreviewBlock[,,] _blockPreviews;
        private int _mainBlockLayer, _subBlocksLayer;

        void InitPreviewer()
        {
            _blockPreviews = new PreviewBlock[10, 10, 10];
            _mainBlockLayer = 1 << LayerMask.NameToLayer("MainBlocks");
            _subBlocksLayer = 1 << LayerMask.NameToLayer("SubBlocks");
        }

        public void UpdateCell(int3 position)
        {
            if (_blockPreviews[position.x, position.y, position.z] != null)
                Destroy(_blockPreviews[position.x, position.y, position.z].gameObject);

            if (Structure.Cells[position.x, position.y, position.z] == null)
                return;

            CellType cellType = getCellType(position);

            PreviewBlock previewBlock = Instantiate(cellType.PreviewCollider, transform);
            previewBlock.Position = position;
            previewBlock.transform.Translate(position.x, position.y, position.z);

            Instantiate(cellType.Block, previewBlock.transform);

            _blockPreviews[position.x, position.y, position.z] = previewBlock;
        }

        private CellType getCellType(int3 position)
        {
            return CellTypes.get()[Structure.Cells[position.x, position.y, position.z].Type.Name];
        }

        //(Position of cell pointed, Position of cell to place)
        public (Option<int3>, Option<int3>) GetPointed(int2 screenPosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Option<int3> mainBlock = Option<int3>.None;
            Option<int3> mainBlockNormal = Option<int3>.None;
            PreviewBlock previewBlock = null;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, _mainBlockLayer))
            {
                previewBlock = hit.collider.gameObject.GetComponentInParent<PreviewBlock>();
                mainBlock = Option<int3>.Some(previewBlock.Position);
                mainBlockNormal = Option<int3>.Some(new int3(Convert.ToInt32(hit.normal.x), Convert.ToInt32(hit.normal.y), Convert.ToInt32(hit.normal.z)));
            }

            Option<int3> subBlock = Option<int3>.None;
            int3 mainBlockNormalVec;
            int3 mainBlockPosition;
            if (mainBlockNormal.IsSome(out mainBlockNormalVec) && mainBlock.IsSome(out mainBlockPosition))
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
            
            return (mainBlock, subBlock);
        }
    }
}
