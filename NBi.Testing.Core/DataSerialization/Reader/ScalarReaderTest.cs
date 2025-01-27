using NBi.Core.DataSerialization.Reader;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.DataSerialization.Reader;

public class ScalarReaderTest
{
    [Test]
    public void Execute_Literal_Returned()
    {
        var json = "{\"glossary\": {\"title\": \"example glossary\"}}";
        var scalarResolver = new LiteralScalarResolver<string>(json);
        var reader = new ScalarReader(scalarResolver);
        var textReader = reader.Execute();

        Assert.That(textReader, Is.Not.Null);
        Assert.That(textReader.ReadToEnd(), Is.EqualTo(json));
    }
}
