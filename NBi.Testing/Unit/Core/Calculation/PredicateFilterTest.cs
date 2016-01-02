using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core;
using NBi.Core.Calculation;
using Moq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;

namespace NBi.Testing.Unit.Core.Calculation
{
    public class PredicateFilterTest
    {
        [Test]
        public void Apply_Resultset_CorrectResult()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("A;10;100");
            rs.Load("B;5;20");
            rs.Load("C;5;50");

            var v1 = Mock.Of<IColumnVariable>(v => v.Column == 1 && v.Name == "a");
            var v2 = Mock.Of<IColumnVariable>(v => v.Column == 2 && v.Name == "b");
            var variables = new List<IColumnVariable>() {v1, v2 };

            var exp = Mock.Of<IColumnExpression>(e => e.Value == "a*b" && e.Name == "c");
            var expressions = new List<IColumnExpression>() { exp };

            var info = Mock.Of<IPredicateInfo>
                (
                    p => p.ComparerType==ComparerType.MoreThanOrEqual
                        && p.ColumnType==ColumnType.Numeric
                        && p.Name == "c"
                        && p.Reference == (object)200
                );

            var filter = new PredicateFilter(variables, expressions, info);
            var result = filter.Apply(rs);

            Assert.That(result.Rows, Has.Count.EqualTo(2));
        }
    }
}
