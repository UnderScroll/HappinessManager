using UnityEngine;

[CreateAssetMenu(fileName = "BlockLimit", menuName = "Rules/BlockLimit")]
public class BlockLimit : IBlockRule
{
    public override string Type => "BlockLimit";
    public override string BaseType => base.Type;

    public CellType BlockToLimit;
    public uint maxAmount;

    public override bool CanPlaceBlock(object value)
    {
        return !(_gameManager.Builder.BlockPlacedAmount[BlockToLimit.Name] + 1 > maxAmount);
    }

    public override bool CanRemoveBlock(object value)
    {
        return true;
    }
}
