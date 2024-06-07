using Builder;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CellType", menuName = "Structure/CellType")]
public class CellType : ScriptableObject
{
    public string Name = "Default";
    public string Description;
    public bool Removable = true;
    public float Mass = 1;
    public float Price = 1;
    public ConnectionType DefaultConnectionType = null;
    public GameObject Block = null;
    public Sprite BlockIcon;
    public PreviewBlock PreviewCollider = null;
    public ConnectionFace ConnectionFaces = ConnectionFace.North | ConnectionFace.East | ConnectionFace.South | ConnectionFace.West | ConnectionFace.Top | ConnectionFace.Bottom;
    public bool ShouldBeSimulated = true;
    public bool IsEmployee = false;

    public static explicit operator CellData(CellType cellType) => new(cellType);
    public static explicit operator EmployeeCellData(CellType cellType) => new(cellType);


    public bool HasConnection(CellData.Face face) => ((int)ConnectionFaces & (1 << (int)face)) != 0;

    [Flags]
    public enum ConnectionFace
    {
        None = 0, 
        North = 1 << 0,
        East = 1 << 1,
        South = 1 << 2,
        West = 1 << 3,
        Top = 1 << 4,
        Bottom = 1 << 5,
    };
}
