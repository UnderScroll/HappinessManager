using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellTypes", menuName = "Structure/CellTypes")]
public class CellTypes : ScriptableObject
{
    [SerializeField]
    private List<CellType> _types;
    private Dictionary<string, CellType> _dictType;

    public List<CellType> Get() => _types;
    public CellType Get(string key) => GetDictionary().TryGetValue(key, out CellType cellType) ? cellType : null;

    private Dictionary<string, CellType> GetDictionary()
    {
        if (_dictType != null) return _dictType;

        //Create a new dictionary
        _dictType = new Dictionary<string, CellType>(_types.Count);

        foreach (CellType cellType in _types)
            _dictType.Add(cellType.Name, cellType);

        return _dictType;
    }

    public CellType Get(int index)
        => index < _types.Count ? _types[index] : null;
}
