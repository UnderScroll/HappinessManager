public readonly struct Option<T>
{
    public static Option<T> None => default;
    public static Option<T> Some(T value) => new(value);

    readonly bool _isSome;
    readonly T _value;

    Option(T value)
    {
        _value = value;
        _isSome = _value is { };
    }

    public bool IsSome(out T value)
    {
        value = _value;
        return _isSome;
    }
}
