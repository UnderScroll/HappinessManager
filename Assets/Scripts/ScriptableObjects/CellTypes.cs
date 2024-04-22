using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellTypes", menuName = "Structure/CellTypes")]
public class CellTypes : ScriptableObject
{
    [SerializeField]
    private List<CellType> _types;
    private Dictionary<string, CellType> _dictType;

    public Dictionary<string, CellType> get()
    {
        if (_dictType != null) return _dictType;

        _dictType = new Dictionary<string, CellType>(_types.Count);
        foreach (CellType cellType in _types)
        {
            _dictType.Add(cellType.Name, cellType);
        }
        return _dictType;
    }
}
