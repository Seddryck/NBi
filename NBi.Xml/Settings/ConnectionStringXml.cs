using NBi.Core.Scalar.Resolver;
using NBi.Xml.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

public class ConnectionStringXml
{
    [XmlText]
    public string Inline { get; set; }

    [XmlElement("environment")]
    public EnvironmentXml Environment { get; set; }

    protected IScalarResolverArgs Args
    {
        get
        {
            if (!string.IsNullOrEmpty(Inline))
                return new LiteralScalarResolverArgs(Inline);
            else if (Environment != null)
                return new EnvironmentScalarResolverArgs(Environment.Name);
            else
                return new LiteralScalarResolverArgs(string.Empty);
        }
    }

    public string GetValue()
    {
        var factory = new ScalarResolverFactory(null);
        var resolver = factory.Instantiate<string>(Args);
        return resolver.Execute();
    }
}
