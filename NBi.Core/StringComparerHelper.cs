namespace NBi.Core;

public class StringComparerHelper
{
    public string Value { get; }

    private StringComparerHelper(string value)
        => Value = value;

    public static StringComparerHelper Build(string value)
        => new (value);

    public override string ToString()
        => Value.ToString();
}
