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
        if (Rules == null)
        {
            Debug.LogError("Rules list was null");
            return;
        }

        foreach (IRule rule in Rules)
        {
            rule._gameManager = _gameManager;

            if (rule.BaseType == "IBlockRule")
                _blockRules.Add((IBlockRule)rule);
        }

        Debug.Log("RuleManager Initialized");
        Debug_DisplayAllRules();
    }

    public void Reset()
    {
        _blockRules.Clear();
    }

    public bool CanPlaceBlock(CellType cellType)
    {
        foreach (IBlockRule rule in _blockRules)
        {
            switch (rule.Type)
            {
                case "BlockLimit":
                    if (!rule.CanPlaceBlock(cellType))
                        return false;
                    break;
                case "BudgetLimit":
                    if (!rule.CanPlaceBlock(cellType.Price))
                        return false;
                    break;
                default:
                    continue;
            }
        }
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
