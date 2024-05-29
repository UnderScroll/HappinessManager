using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BudgetLimitRule", menuName = "Rules/BudgetLimit")]
public class BudgetLimit : IBlockRule
{
    public override string Type => "BudgetLimit";
    public override string BaseType => base.Type;
    public float Budget;

    public override bool CanPlaceBlock(float blockPrice)
    {
        return !(_gameManager.Builder.SpentMoney + blockPrice > Budget);
    }

    public override bool CanRemoveBlock(float blockPrice)
    {
        return true;
    }


}
