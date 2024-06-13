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
        if (value is CellType && ((CellType)value).Name == BlockToLimit.Name)
            return !(_gameManager.Builder.BlockPlacedAmount[BlockToLimit.Name] + 1 > maxAmount);

        return true;
    }

    public override bool CanRemoveBlock(object value)
    {
        return true;
    }

    public override bool Validate()
    {
        return _gameManager.Builder.BlockPlacedAmount[BlockToLimit.Name] <= maxAmount;
    }
}
