using Builder;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Structure/Level")]
public class Level : ScriptableObject
{
    //Structure
    public Structure Structure;

    //Cells
    public CellTypes CellTypes;
    public CellTypes PlaceableCellTypes;

    //Wind
    public bool IsWindEnabled;
    public Vector3 WindDirection;
    public float WindStrength;

    //Rules
    public List<IRule> Rules;
}