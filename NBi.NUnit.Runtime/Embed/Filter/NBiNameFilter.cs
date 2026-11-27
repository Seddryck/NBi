using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NBi.NUnit.Runtime.Embed.Filter;

[Serializable]
public class NBiNameFilter(string name) : TestFilter
{
    public override bool Match(ITest test) => test.Name == name;

    public override TNode AddToXml(TNode parentNode, bool recursive)
        => parentNode.AddElement("name", name);
}
