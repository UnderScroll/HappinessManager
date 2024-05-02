using Builder;
using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CellType", menuName = "Structure/CellType")]
public class CellType : ScriptableObject
{
    public string Name = "Default";
    public bool Removable = true;
    public float Mass = 1;
    public ConnectionType DefaultConnectionType = null;
    public GameObject Block = null;
    public PreviewBlock PreviewCollider = null;
    public ConnectionFace ConnectionFaces = ConnectionFace.North | ConnectionFace.East | ConnectionFace.South | ConnectionFace.West | ConnectionFace.Top | ConnectionFace.Bottom;

    public static explicit operator Builder.CellData(CellType cellType) => new Builder.CellData(cellType);

    public bool hasConnection(CellData.Face face) => ((int)ConnectionFaces & (1 << (int)face)) != 0;

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
