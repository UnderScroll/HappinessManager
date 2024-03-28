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
        Fixed,
    }

    public Block block;
    public Type type;
}
