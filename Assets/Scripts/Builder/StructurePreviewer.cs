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
        private int _previewLayer;

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

        //(Position of cell hit, Position of cell to place hit)
        public (Option<int3>, Option<int3>) GetPointed(int2 screenPosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, float.PositiveInfinity, _previewLayer))
                return (Option<int3>.None, Option<int3>.None);

            PreviewBlock previewBlock = hit.collider.gameObject.GetComponentInParent<PreviewBlock>();
            switch (hit.collider.name)
            {
                case "MainCollider":
                    int3 cellToPlacePositon = new int3(Convert.ToInt32(hit.normal.x), Convert.ToInt32(hit.normal.y), Convert.ToInt32(hit.normal.z)) + previewBlock.Position;

                    if (!IsInBounds(cellToPlacePositon))
                        return (Option<int3>.Some(previewBlock.Position), Option<int3>.None);
                    else
                        return (Option<int3>.Some(previewBlock.Position), Option<int3>.Some(cellToPlacePositon));
                case "BottomCollider":
                    if (previewBlock.Position.y > 0)
                        return (Option<int3>.Some(previewBlock.Position), Option<int3>.Some(new int3(previewBlock.Position.x, previewBlock.Position.y - 1, previewBlock.Position.z)));
                    else
                        return (Option<int3>.None, Option<int3>.None);
                default:
                    return (Option<int3>.None, Option<int3>.None);
            }
        }
    }
}
