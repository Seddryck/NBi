using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver
{
    public class NCalcScalarResolverTest
    {
        [Test]
        public void Instantiate_WithoutRowValues_CorrectComputation()
        {
            using (var dt = new DataTableResultSet())
            {
                var row = dt.NewRow();
                var context = new Context();
                var args = new NCalcScalarResolverArgs("1+1", context);
                context.Switch(row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnNames_CorrectComputation()
        {
            using (var dt = new DataTableResultSet())
            {
                dt.AddColumn("a", typeof(int));
                dt.AddColumn("b", typeof(int));
                dt.AddColumn("c", typeof(int));
                var row = dt.NewRow();
                var context = new Context();
                row.ItemArray = [2, 5, 3];
                var args = new NCalcScalarResolverArgs("a*Max(b, c)-2", context);
                context.Switch(row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2*Math.Max(5,3)-2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnBracketNames_CorrectComputation()
        {
            using (var dt = new DataTableResultSet())
            {
                dt.AddColumn("a", typeof(int));
                dt.AddColumn("b", typeof(int));
                dt.AddColumn("c", typeof(int));
                var row = dt.NewRow();
                row.ItemArray = [2, 5, 3];
                var context = new Context();
                var args = new NCalcScalarResolverArgs("[a]*Max([b], [c])-2", context);
                context.Switch(row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2 * Math.Max(5, 3) - 2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnOrdinals_CorrectComputation()
        {
            using (var dt = new DataTableResultSet())
            {
                dt.AddColumn("a", typeof(int));
                dt.AddColumn("b", typeof(int));
                dt.AddColumn("c", typeof(int));
                var row = dt.NewRow();
                row.ItemArray = [2, 5, 3];
                var context = new Context();
                var args = new NCalcScalarResolverArgs("[#0]*Max([#1], [#2])-2", context);
                context.Switch(row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2 * Math.Max(5, 3) - 2));
            }
        }

        [Test]
        public void Instantiate_WithRowValuesBasedOnOrdinalsAndVariable_CorrectComputation()
        {
            using (var dt = new DataTableResultSet())
            {
                dt.AddColumn("a", typeof(int));
                dt.AddColumn("b", typeof(int));
                dt.AddColumn("c", typeof(int));
                var row = dt.NewRow();
                row.ItemArray = [2, 5, 3];
                var context = new Context();
                context.Variables.Add<decimal>("myVar", 10m);
                var args = new NCalcScalarResolverArgs("[#0]*Max([#1], [#2])-[@myVar]", context);
                context.Switch(row);
                var resolver = new NCalcScalarResolver<object>(args);

                var output = resolver.Execute();

                Assert.That(output, Is.EqualTo(2 * Math.Max(5, 3) - 10));
            }
        }

    }
}
