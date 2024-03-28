using UnityEngine;
using System;
using System.Collections.Generic;

public class Structure : MonoBehaviour
{
    static public PreviewBlock previewBlock;
    public Cell[,,] cells { get; }

    public Structure()
        :this(1, 1, 1)
    {}

    public Structure(uint width, uint height, uint depth)
    {
        cells = new Cell[width, height, depth];
        for (uint x = 0; x < width; x++)
            for (uint y = 0; y < height; y++)
                for (uint z = 0; z < depth; z++)
                {
                    cells[x, y, z] = new Cell();
                }
    }

    public bool isInBounds(uint x, uint y, uint z)
    {
        return x >= 0 && x < cells.GetLength(0)
            && y >= 0 && y < cells.GetLength(1)
            && z >= 0 && z < cells.GetLength(2);
    }

    public bool placeBlock(Block block, uint x, uint y, uint z)
    {
        if (!isInBounds(x, y, z))
            return false;

        if (cells[x, y, z].type != Cell.Type.Empty)
            return false;

        block = Instantiate(block);

        Structure structure = this;
        return block.place(ref structure, x, y, z);
    }

    public bool placePreviewBlock(uint x, uint y, uint z)
    {
        return placeBlock(previewBlock, x, y, z);
    }
}
