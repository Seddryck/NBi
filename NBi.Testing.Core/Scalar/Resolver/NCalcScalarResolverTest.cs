using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Scalar.Resolver
{
    public class NCalcScalarResolverTest
    {
        [Test]
        public void Instantiate_WithoutRowValues_CorrectComputation()
        {
            using (var dt = new DataTable())
            {
                var row = dt.NewRow();
                var args = new NCalcScalarResolverArgs("1+1", row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnNames_CorrectComputation()
        {
            using (var dt = new DataTable())
            {
                dt.Columns.Add("a", typeof(int));
                dt.Columns.Add("b", typeof(int));
                dt.Columns.Add("c", typeof(int));
                var row = dt.NewRow();
                row.ItemArray = new object[] { 2, 5, 3 };
                var args = new NCalcScalarResolverArgs("a*Max(b, c)-2", row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2*Math.Max(5,3)-2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnBracketNames_CorrectComputation()
        {
            using (var dt = new DataTable())
            {
                dt.Columns.Add("a", typeof(int));
                dt.Columns.Add("b", typeof(int));
                dt.Columns.Add("c", typeof(int));
                var row = dt.NewRow();
                row.ItemArray = new object[] { 2, 5, 3 };
                var args = new NCalcScalarResolverArgs("[a]*Max([b], [c])-2", row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2 * Math.Max(5, 3) - 2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnOrdinals_CorrectComputation()
        {
            using (var dt = new DataTable())
            {
                dt.Columns.Add("a", typeof(int));
                dt.Columns.Add("b", typeof(int));
                dt.Columns.Add("c", typeof(int));
                var row = dt.NewRow();
                row.ItemArray = new object[] { 2, 5, 3 };
                var args = new NCalcScalarResolverArgs("[#0]*Max([#1], [#2])-2", row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2 * Math.Max(5, 3) - 2));
            }
        }

    }
}
