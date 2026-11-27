using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NBi.NUnit.Runtime.Embed.Filter;

[Serializable]
public class PropertyFilter(string name, string value) : TestFilter
{
    protected string Name { get; } = name;
    protected string Value { get; } = value;

    public override bool Match(ITest test)
        => test.Properties.ContainsKey(Name) && test.Properties[Name].Contains(Value);

    public override TNode AddToXml(TNode parentNode, bool recursive)
    {
        var element = parentNode.AddElement("prop", Value);
        element.AddAttribute("name", Name);
        return element;
    }

    public override string ToString() => $"{Name}::{Value}";
}
