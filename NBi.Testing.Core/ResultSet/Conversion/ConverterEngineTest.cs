using NBi.Core.ResultSet.Conversion;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Conversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Conversion
{
    public class ConverterEngineTest
    {
        [Test]
        public void Execute_FirstColumnIsText_FirstColumnIsNumeric()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "100,12", "Alpha" }, new[] { "100", "Beta" }, new[] { "0,1", "Gamma" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var factory = new ConverterFactory();
            var converter = factory.Instantiate("text", "numeric", null, "fr-fr");
            Assert.That(converter, Is.Not.Null);
            Assert.That(converter, Is.TypeOf<TextToNumericConverter>());

            var engine = new ConverterEngine("#0", converter);
            engine.Execute(rs);
            Assert.That(rs, Is.Not.Null);
            Assert.That(rs.GetColumn(0)?.DataType, Is.EqualTo(typeof(decimal)));
            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs[0][0], Is.EqualTo(100.12));
            Assert.That(rs[1][0], Is.EqualTo(100));
            Assert.That(rs[2][0], Is.EqualTo(0.1));
        }

        [Test]
        public void Execute_LastColumnIsText_LastColumnIsNumeric()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "Alpha", "100,12" }, new[] { "Beta", "100" }, new[] { "Gamma", "0,1" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var factory = new ConverterFactory();
            var converter = factory.Instantiate("text", "numeric", null, "fr-fr");
            Assert.That(converter, Is.Not.Null);
            Assert.That(converter, Is.TypeOf<TextToNumericConverter>());

            var engine = new ConverterEngine("#1", converter);
            engine.Execute(rs);
            Assert.That(rs, Is.Not.Null);
            Assert.That(rs.GetColumn(1)?.DataType, Is.EqualTo(typeof(decimal)));
            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs[0][1], Is.EqualTo(100.12));
            Assert.That(rs[1][1], Is.EqualTo(100));
            Assert.That(rs[2][1], Is.EqualTo(0.1));
        }

        [Test]
        public void Execute_MiddleColumnIsText_MiddleColumnIsNumeric()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "Alpha", "100,12", "true" }, new[] { "Beta", "100", "false" }, new[] { "Gamma", "N/A", "false" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var factory = new ConverterFactory();
            var converter = factory.Instantiate("text", "numeric", null, "fr-fr");
            Assert.That(converter, Is.Not.Null);
            Assert.That(converter, Is.TypeOf<TextToNumericConverter>());

            var engine = new ConverterEngine("#1", converter);
            engine.Execute(rs);
            Assert.That(rs, Is.Not.Null);
            Assert.That(rs.GetColumn(1)?.DataType, Is.EqualTo(typeof(decimal)));
            Assert.That(rs.ColumnCount, Is.EqualTo(3));
            Assert.That(rs[0][1], Is.EqualTo(100.12));
            Assert.That(rs[1][1], Is.EqualTo(100));
            Assert.That(rs[2][1], Is.EqualTo(DBNull.Value));
        }

        [Test]
        public void Execute_MiddleColumnIsTextualDate_MiddleColumnIsDate()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "Alpha", "06/01/2018", "true" }, new[] { "Beta", "17/12/2015", "false" }, new[] { "Gamma", "Before 2014", "false" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var factory = new ConverterFactory();
            var converter = factory.Instantiate("text", "date", new DateTime(2013,1,1), "fr-fr");
            Assert.That(converter, Is.Not.Null);
            Assert.That(converter, Is.TypeOf<TextToDateConverter>());

            var engine = new ConverterEngine("#1", converter);
            engine.Execute(rs);
            Assert.That(rs, Is.Not.Null);
            Assert.That(rs.GetColumn(1)?.DataType, Is.EqualTo(typeof(DateTime)));
            Assert.That(rs.ColumnCount, Is.EqualTo(3));
            Assert.That(rs[0][1], Is.EqualTo(new DateTime(2018,1,6)));
            Assert.That(rs[1][1], Is.EqualTo(new DateTime(2015, 12, 17)));
            Assert.That(rs[2][1], Is.EqualTo(new DateTime(2013, 1, 1)));
        }

        [Test]
        public void Execute_MiddleColumnIsTextualDateTime_MiddleColumnIsDateTime()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new[] { "Alpha", "06/01/2018 08:12:00", "true" }, new[] { "Beta", "17/12/2015 08:12:00", "false" }, new[] { "Gamma", "Before 2014", "false" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var factory = new ConverterFactory();
            var converter = factory.Instantiate("text", "dateTime", new DateTime(2019, 12, 31, 23, 59, 59), "fr-fr");
            Assert.That(converter, Is.Not.Null);
            Assert.That(converter, Is.TypeOf<TextToDateTimeConverter>());

            var engine = new ConverterEngine("#1", converter);
            engine.Execute(rs);
            Assert.That(rs, Is.Not.Null);
            Assert.That(rs.GetColumn(1)?.DataType, Is.EqualTo(typeof(DateTime)));
            Assert.That(rs.ColumnCount, Is.EqualTo(3));
            Assert.That(rs[0][1], Is.EqualTo(new DateTime(2018, 1, 6, 8,12,0)));
            Assert.That(rs[1][1], Is.EqualTo(new DateTime(2015, 12, 17, 8, 12, 0)));
            Assert.That(rs[2][1], Is.EqualTo(new DateTime(2019, 12, 31, 23, 59, 59)));
        }
    }
}
