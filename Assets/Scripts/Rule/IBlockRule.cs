public abstract class IBlockRule : IRule
{
    public override string Type => "IBlockRule";
    public override string BaseType => base.Type;

    public abstract bool CanPlaceBlock(object value);

    public abstract bool CanRemoveBlock(object value);
}
