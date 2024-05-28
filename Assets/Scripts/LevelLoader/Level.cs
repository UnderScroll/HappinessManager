using Builder;
using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Structure/Level")]
public class Level : ScriptableObject
{
    public Structure Structure;
    public CellTypes CellTypes;
    public CellTypes PlaceableCellTypes;
    public Vector3 WindDirection;
    public float WindStrength;
}
