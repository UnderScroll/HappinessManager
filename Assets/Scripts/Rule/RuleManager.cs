using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    [HideInInspector]
    public List<IRule> Rules;
    private GameManager _gameManager;

    private List<IBlockRule> _blockRules;

    public void Awake()
    {
        if (!TryGetComponent(out _gameManager))
            Debug.LogError("Failted to get the Game Manager in RuleManager");

        _blockRules = new();
    }

    public void Initialize()
    {
        foreach (IRule rule in Rules)
        {
            rule._gameManager = _gameManager;

            if (rule.BaseType == "IBlockRule")
                _blockRules.Add((IBlockRule)rule);
        }
    }

    public void Reset()
    {
        _blockRules.Clear();
    }

    public bool CanPlaceBlock(float blockPrice)
    {
        foreach (IBlockRule rule in _blockRules)
            if (!rule.CanPlaceBlock(blockPrice))
                return false;
        return true;
    }

    public bool CanRemoveBlock(float blockPrice)
    {
        foreach (IBlockRule rule in _blockRules)
            if (!rule.CanRemoveBlock(blockPrice))
                return false;
        return true;
    }

    public void Debug_DisplayAllRules()
    {
        foreach (IRule rule in Rules)
        {
            Debug.Log($"Type : {rule.Type}, Base {rule.BaseType}");
        }
    }
}
