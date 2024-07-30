using NBi.Core.DataSerialization.Flattening;
using NBi.Core.DataSerialization.Flattening.Json;
using NBi.Core.DataSerialization.Reader;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Resolver
{
    public class DataSerializationResultSetResolverTest
    {
        [Test()]
        public void Instantiate_ColumnsBased_CorrectType()
        {
            var json = "{\"glossary\": {\"title\": \"example glossary\"}}";
            var args = new DataSerializationResultSetResolverArgs(
                new ScalarReaderArgs(new LiteralScalarResolver<string>(json)),
                new JsonPathArgs()
                {
                    From = new LiteralScalarResolver<string>("$"),
                    Selects = [new ElementSelect(new LiteralScalarResolver<string>("$.glossary.title"))]
                }
            );
            var resolver = new DataSerializationResultSetResolver(args);

            var rs = resolver.Execute();
            Assert.That(rs.ColumnCount, Is.EqualTo(1));
            Assert.That(rs.Rows.Count, Is.EqualTo(1));
            Assert.That(rs[0][0], Is.EqualTo("example glossary"));
        }
    }
}
