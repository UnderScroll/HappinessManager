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

    public static explicit operator Builder.CellData(CellType cellType) => new Builder.CellData(cellType);
}
