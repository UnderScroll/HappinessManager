using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

namespace Builder
{
    public partial class Builder : MonoBehaviour
    {
        [HideInInspector]
        public Level Level;
        private CellData _selectedBlock;

        private (Option<int3> pointedCell, Option<int3> cellToPlace) _pointed;

        [HideInInspector]
        public float SpentMoney = 0;
        [HideInInspector]
        public Dictionary<string, uint> BlockPlacedAmount;

        private GameManager _gameManager;

        private readonly int3[] _neighborDeltaPositions = {
                new( 0, 0, 1),
                new( 1, 0, 0),
                new( 0, 0,-1),
                new(-1, 0, 0),
                new( 0, 1, 0),
                new( 0,-1, 0)
            };

        private readonly int[] _inverseFaceOrder = { 2, 3, 0, 1, 5, 4 };


        public void Initialize()
        {
            //Initialize the previewer
            InitializePreviewer();

            //Select the first block in placeableBlocks
            SelectBlock(0);
            if (_selectedBlock == null)
                Debug.LogError("Failed to select first block, check if CellTypes is null or empty");

            SpentMoney = 0;
            BlockPlacedAmount = new();

            foreach (CellType placeableCellType in Level.PlaceableCellTypes.Get())
                BlockPlacedAmount.Add(placeableCellType.Name, 0);
        }

        void PlaceBlock(CellData data, int3 position)
        {
            Level.Structure.Cells[position.x, position.y, position.z] = data;
            data.Position = position;

            UpdateCell(position);
            AkSoundEngine.PostEvent("Play_Build_basic_block_place", gameObject);
        }

        void RemoveBlock(int3 position)
        {
            CellData cell = Level.Structure.Cells[position.x, position.y, position.z];

            if (cell.Type.Removable)
            {
                Level.Structure.Cells[position.x, position.y, position.z] = null;

                UpdateCell(position);
                AkSoundEngine.PostEvent("Play_Build_basic_block_remove", gameObject);
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
                && position.x < Level.Structure.Cells.GetLength(0)
                && position.y < Level.Structure.Cells.GetLength(1)
                && position.z < Level.Structure.Cells.GetLength(2);
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

                int3 neighborPosition = _neighborDeltaPositions[i] + cellData.Position;
                if (!IsInBounds(neighborPosition))
                    continue;

                CellData neighborCellData = Level.Structure.Cells[neighborPosition.x, neighborPosition.y, neighborPosition.z];
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

            CellData cellData = Level.Structure.Cells[position.x, position.y, position.z];

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

                CellData neighborCellData = Level.Structure.Cells[neighborPosition.x, neighborPosition.y, neighborPosition.z];
                if (neighborCellData == null)
                    continue;

                neighborCellData.UpdateConnection((CellData.Face)_inverseFaceOrder[i], neighborCellData.Type.DefaultConnectionType);
            }
        }

        public void OnPlace(InputValue _)
        {
            if (!_pointed.cellToPlace.IsSome(out int3 positionToPlace))
                return;

            if (Level.Structure.Cells[positionToPlace.x, positionToPlace.y, positionToPlace.z] != null)
                return;

            if (!_gameManager.RuleManager.CanPlaceBlock(_selectedBlock.Type.Price))
                return;

            PlaceBlock(_selectedBlock.Clone(), positionToPlace);
            UpdateConnection(positionToPlace);

            SpentMoney += _selectedBlock.Type.Price;
            BlockPlacedAmount[_selectedBlock.Type.Name]++;

            PreviewBlock blockInstance = _previewBlocks[positionToPlace.x, positionToPlace.y, positionToPlace.z]; //Get block in scene
            if (!blockInstance.TryGetComponent(out BlockSoundPlayer soundPlayer)) //Try to get the script
                Debug.LogWarning("NoSoundPlayer"); //If not warn
            else //If script present => play sound
                soundPlayer.PlayBasicBlockPlace();

            return;
        }

        public void OnRemove(InputValue _)
        {
            if (!_pointed.pointedCell.IsSome(out int3 positionToRemove))
                return;

            if (_pointed.cellToPlace.IsSome(out int3 positionToPlace)
                && (positionToPlace.y == positionToRemove.y - 1))
                    return;
            
            CellType removedBlockType = Level.Structure.Cells[positionToRemove.x, positionToRemove.y, positionToRemove.z].Type;
            
            PreviewBlock blockInstance = _previewBlocks[positionToRemove.x, positionToRemove.y, positionToRemove.z]; //Get block in scene
            if (!blockInstance.TryGetComponent(out BlockSoundPlayer soundPlayer)) //Try to get the script
                Debug.LogWarning("NoSoundPlayer"); //If not warn
            else //If script present => play sound
                soundPlayer.PlayBasicBlockRemove();

            RemoveBlock(positionToRemove);
            UpdateConnection(positionToRemove);

            SpentMoney -= removedBlockType.Price;
            BlockPlacedAmount[removedBlockType.Name]--;
        }

        public void OnSelectBlock1(InputValue _) => SelectBlock(0);
        public void OnSelectBlock2(InputValue _) => SelectBlock(1);
        public void OnSelectBlock3(InputValue _) => SelectBlock(2);
        public void OnSelectBlock4(InputValue _) => SelectBlock(3);

        public void SelectBlock(uint index)
        {
            if (index < Level.PlaceableCellTypes.Get().Count)
                _selectedBlock = (CellData)Level.PlaceableCellTypes[(int)index];
        }
    }
}

