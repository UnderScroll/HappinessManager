using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Structure : MonoBehaviour
{
    static public PreviewBlock previewBlock;

    static public Transform origin;

    public Cell[,,] cells { get; }

    public Structure()
        : this(1, 1, 1)
    { }

    public Structure(uint width, uint height, uint depth)
    {
        cells = new Cell[width, height, depth];
        for (uint x = 0; x < width; x++)
            for (uint y = 0; y < height; y++)
                for (uint z = 0; z < depth; z++)
                {
                    cells[x, y, z] = new Cell
                    {
                        position = (x, y, z),
                        type = Cell.Type.Empty
                    };
                }
    }

    public bool isInBounds((uint x, uint y, uint z) position)
    {
        return position.x >= 0 && position.x < cells.GetLength(0)
            && position.y >= 0 && position.y < cells.GetLength(1)
            && position.z >= 0 && position.z < cells.GetLength(2);
    }

    public bool placeBlockAndUpdatePreview(Block block, (uint x, uint y, uint z) position)
    {
        if (placeBlock(block, position))
        {
            Cell updatedCell = cells[position.x, position.y, position.z];
            Cell[] updatedCells = { updatedCell };

            return updatePreviewBlocks(updatedCells);
        }

        return false;
    }

    public bool removeBlockAndUpdatePreview((uint x, uint y, uint z) position)
    {
        Cell[] updatedCells = { cells[position.x, position.y, position.z] };
        if (removeBlock(position))
            return updatePreviewBlocks(updatedCells);

        return false;
    }
    public bool placeBlock(Block block, (uint x, uint y, uint z) position)
    {
        if (!isInBounds(position))
            return false;

        Block blockInstance = Instantiate(block, origin);
        if (!blockInstance.place(this, position))
        {
            Destroy(blockInstance);
            return false;
        }

        return true;
    }

    public bool connectBlockToNeighbors(Block block)
    {
        List<Cell> neighbors = block.getNeighbors(this);
        foreach (Cell neighbor in neighbors)
        {
            switch (neighbor.type)
            {
                case Cell.Type.Full:
                    connectBlockToNeighbor(block, neighbor.block);
                    break;
                default:
                    break;
            }
        }

        return true;
    }

    bool connectBlockToNeighbor(Block block, Block neighbor)
    {
        //If block is already connected to neighbor, return false
        if (block.connectedBlocks.Contains(neighbor))
            return false;

        //If neighbor is already connected to block, return false
        if (neighbor.connectedBlocks.Contains(block))
            return false;

        //Add neightbor to block
        block.connectedBlocks.Add(neighbor);
        
        return true;
    }

    public bool removeBlock((uint x, uint y, uint z) position)
    {
        Cell cell = cells[position.x, position.y, position.z];

        return cell.block.remove(this);
    }

    public bool updatePreviewBlocks(Cell[] updatedCells)
    {
        foreach (Cell updatedCell in updatedCells)
        {
            switch (updatedCell.type)
            {
                case Cell.Type.Full:
                    onFullBlockUpdated(updatedCell);
                    break;
                case Cell.Type.Empty:
                case Cell.Type.Employee:
                    onEmptyCellUpdated(updatedCell);
                    break;
                default:
                    break;
            }
        }

        return true;
    }

    private void onFullBlockUpdated(Cell updatedCell)
    {
        List<Cell> neighbors = updatedCell.block.getNeighbors(this);
        foreach (Cell neighbor in neighbors)
            switch (neighbor.type)
            {
                case Cell.Type.Empty:
                    placePreviewBlock(neighbor.position);
                    break;
                default:
                    break;
            }
    }

    private void onEmptyCellUpdated(Cell updatedCell)
    {
        List<Cell> neighbors = updatedCell.getNeighbors(this);
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor.type == Cell.Type.Preview)
            {
                PreviewBlock previewBlock = neighbor.block.GetComponent<PreviewBlock>();
                if (previewBlock.getNeighbors(this).FindAll(x => x.type == Cell.Type.Full).Count == 0)
                    removeBlock((previewBlock.position.x, previewBlock.position.y, previewBlock.position.z));
            }
        }

        if (updatedCell.getNeighbors(this).FindAll(x => x.type == Cell.Type.Full).Count > 0)
            placePreviewBlock((updatedCell.position.x, updatedCell.position.y, updatedCell.position.z));
    }

    public bool placePreviewBlock((uint x, uint y, uint z) position)
    {
        return placeBlock(previewBlock, position);
    }
}
