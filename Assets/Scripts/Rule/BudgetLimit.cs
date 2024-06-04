using UnityEngine;

[CreateAssetMenu(fileName = "BudgetLimitRule", menuName = "Rules/BudgetLimit")]
public class BudgetLimit : IBlockRule
{
    public override string Type => "BudgetLimit";
    public override string BaseType => base.Type;
    public float Budget;

    public override bool CanPlaceBlock(object value)
    {
        return !(_gameManager.Builder.SpentMoney + (float)value > Budget);
    }

    public override bool CanRemoveBlock(object value)
    {
        return true;
    }


}
