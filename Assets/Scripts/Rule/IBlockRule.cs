public abstract class IBlockRule : IRule
{
    public override string Type => "IBlockRule";
    public override string BaseType => base.Type;

    public abstract bool CanPlaceBlock(float blockPrice);

    public abstract bool CanRemoveBlock(float blockPrice);
}
