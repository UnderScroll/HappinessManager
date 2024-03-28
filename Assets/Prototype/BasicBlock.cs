using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBlock : Block
{
    public override bool place(ref Structure structure, uint x, uint y, uint z)
    {
        transform.position = new Vector3(x, y, z);
        position = (x, y, z);

        Block block = this;
        structure.cells[x, y, z].block = block;

        structure.cells[x, y, z].type = Cell.Type.Full;

        structure.placePreviewBlock(x + 1, y, z);
        structure.placePreviewBlock(x - 1, y, z);
        structure.placePreviewBlock(x, y + 1, z);
        structure.placePreviewBlock(x, y - 1, z);
        structure.placePreviewBlock(x, y, z + 1);
        structure.placePreviewBlock(x, y, z - 1);

        return true;
    }

    public override bool remove(ref Structure structure, uint x, uint y, uint z)
    {
        throw new System.NotImplementedException();
    }
}
