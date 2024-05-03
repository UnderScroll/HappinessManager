using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FixedBlock : Block
{
    public override bool place(Structure structure, (uint x, uint y, uint z) position)
    {
        transform.localPosition = new Vector3(position.x, position.y, position.z);
        this.position = position;

        Block block = this;
        Cell cell = structure.cells[position.x, position.y, position.z];

        cell.block = block;
        cell.type = Cell.Type.Full;
        cell.removable = false;

        return true;
    }

    public override List<Cell> getNeighbors(Structure structure)
    {
        List<Cell> neighbours = new List<Cell>();

        (uint x, uint y, uint z)[] neighbourPositions = {
            (position.x - 1, position.y, position.z),
            (position.x + 1, position.y, position.z),
            (position.x, position.y - 1, position.z), 
            (position.x, position.y + 1, position.z), 
            (position.x, position.y, position.z - 1), 
            (position.x, position.y, position.z + 1) 
        };

        foreach ((uint x, uint y, uint z) position in neighbourPositions)
        {
            if (!structure.isInBounds(position))
                continue;

            neighbours.Add(structure.cells[position.x, position.y, position.z]);
        }

        return neighbours;
    }

    public override bool remove(Structure structure)
    {
        return false;
    }
}