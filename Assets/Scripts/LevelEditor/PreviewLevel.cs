using Builder;
using LevelLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PreviewLevel
{
    Structure Structure;
    List<CellType> Types;
    List<CellType> PlaceableTypes;

    public LevelLoader.Level CreateScriptableObject()
    {
        throw new NotImplementedException();
    }

    public void Load(Level level)
    {
        throw new NotImplementedException();
    }
}
