using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

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
            Cell[] updatedCells = { cells[position.x, position.y, position.z] };
            updatePreviewBlocks(updatedCells);
            return false;
        }

        return false;
    }

    public bool placeBlock(Block block, (uint x, uint y, uint z) position)
    {
        if (!isInBounds(position))
            return false;

        Structure structure = this;
        block = Instantiate(block, origin);
        if (block.place(ref structure, position))
        {
            return true;
        }

        Destroy(block);
        return false;
    }

    public bool removeBlock((uint x, uint y, uint z) position)
    {
        Cell cell = cells[position.x, position.y, position.z];
        Structure structure = this;

        return cell.block.remove(ref structure);
    }

    public bool updatePreviewBlocks(Cell[] updatedCells)
    {
        Structure structure = this;

        foreach (Cell updatedCell in updatedCells)
        {
            switch (updatedCell.type)
            {
                case Cell.Type.Full:
                    onFullBlockUpdated(updatedCell);
                    break;
                case Cell.Type.Empty:
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
        Structure structure = this;
        List<Cell> neighbors = updatedCell.block.getNeighbors(ref structure);
        foreach (Cell neighbor in neighbors)
            switch (neighbor.type)
            {
                case Cell.Type.Empty:
                    placePreviewBlock(neighbor.position);
                    break;
                case Cell.Type.Preview:
                    break;
                default:
                    break;
            }
    }

    private void onEmptyCellUpdated(Cell updatedCell)
    {
        Structure structure = this;
        List<Cell> neighbors = updatedCell.getNeighbors(ref structure);
        foreach (Cell neighbor in neighbors)
        {
            switch (neighbor.type)
            {
                case Cell.Type.Full:
                    placePreviewBlock(updatedCell.position);
                    break;
                case Cell.Type.Preview:
                    PreviewBlock previewBlock = neighbor.block.GetComponent<PreviewBlock>();
                    if (previewBlock.getNeighbors(ref structure).FindAll(x => x.type == Cell.Type.Full).Count == 0)
                        removeBlock((previewBlock.position.x, previewBlock.position.y, previewBlock.position.z));
                    break;
            }
        }
    }

    public bool placePreviewBlock((uint x, uint y, uint z) position)
    {
        return placeBlock(previewBlock, position);
    }


}
