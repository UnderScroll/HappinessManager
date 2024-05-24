using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class CellTypes
{
    [SerializeField]
    private List<CellType> _types;
    private Dictionary<string, CellType> _dictType;

    public CellTypes() => _types = new();

    public List<CellType> Get() => _types;

    public CellType this[int index]
    {
        get { return _types[index]; }
        set {
            GetDictionary().Remove(_types[index].Name);
            _types[index] = value;
            GetDictionary().Add(_types[index].Name, value);
        }
    }
    public CellType this[string key]
    {
        get { return GetDictionary()[key]; }
        set
        {
            if (!TryGetValue(value.Name, out var _))
            {
                GetDictionary().Add(value.Name, value);
                _types.Add(value);
            }
        }
    }

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

    public bool TryGetValue(string key, out CellType value)
    {
        value = null;
        if (key == null)
            return false;

        return GetDictionary().TryGetValue(key, out value);
    }

    public bool Remove(string key)
    {
        bool success = _types.Remove(GetDictionary()[key]);
        if (success)
            success = GetDictionary().Remove(key);
        
        return success;
    }
}
