using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBlock : Block
{
    private new void Awake(){}

    public override bool place(ref Structure structure, uint x, uint y, uint z)
    {
        transform.position = new Vector3(x, y, z);
        position = (x, y, z);
        
        Block block = this; 
        structure.cells[x, y, z].block = block;

        structure.cells[x, y, z].type = Cell.Type.Empty;

        return true;
    }

    public override bool remove(ref Structure structure, uint x, uint y, uint z)
    {
        throw new System.NotImplementedException();
    }
}
