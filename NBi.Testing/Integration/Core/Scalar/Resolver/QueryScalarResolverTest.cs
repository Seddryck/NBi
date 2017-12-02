using Moq;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.Scalar.Resolver
{
    public class QueryScalarResolverTest
    {
        [Test]
        public void Execute_Query_IsExecuted()
        {
            var args = new QueryScalarResolverArgs(
                new QueryResolverArgs("select 10;", ConnectionStringReader.GetSqlClient(), null, null, new TimeSpan(0,0,10), System.Data.CommandType.Text)
                );

            var resolver = new QueryScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(10));
        }
    }
}
