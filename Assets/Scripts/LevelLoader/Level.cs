using Builder;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


namespace LevelLoader
{
    [CreateAssetMenu(fileName = "Level", menuName = "Structure/Level")]

    public class Level : ScriptableObject
    {
        public Structure Structure;
        public CellTypes CellTypes;
        public CellTypes PlaceableCellTypes;
    }
}
