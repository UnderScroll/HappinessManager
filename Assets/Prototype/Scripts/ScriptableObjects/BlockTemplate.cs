using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Structure/Block")]

public class BlockTemplate : ScriptableObject
{
    Faces faces;

    [Flags]
    enum Faces
    {
        North,
        East,
        South,
        West,
        Top,
        Bottom
    }

    public static implicit operator Block(BlockTemplate template)
    {
        throw new NotImplementedException("Implicit Cast to Block was not implemented");
    }
}
