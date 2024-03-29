using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public enum Type
    {
        Empty,
        Half,
        Full,
        Preview,
    }

    public Block block;
    public Type type;
    public bool removable;
    public (uint x, uint y, uint z) position;

    public List<Cell> getNeighbors(ref Structure structure)
    {
        List<Cell> neighbors = new List<Cell>();

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

            neighbors.Add(structure.cells[position.x, position.y, position.z]);
        }

        return neighbors;
    }
}
