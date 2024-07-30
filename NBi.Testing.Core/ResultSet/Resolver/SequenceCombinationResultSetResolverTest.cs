using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;

namespace NBi.Core.Testing.ResultSet.Resolver
{
    public class SequenceCartesianProductResultSetResolverTest
    {
        [Test()]
        public void Instantiate_OneSequence_CorrectType()
        {
            var sequenceResolver = new ListSequenceResolver<DateTime>(
                new ListSequenceResolverArgs(new[] {
                    new LiteralScalarResolver<string>("2015-01-01"),
                    new LiteralScalarResolver<string>("2016-01-01"),
                    new LiteralScalarResolver<string>("2017-01-01"),
                })
            );

            var args = new SequenceCombinationResultSetResolverArgs(new[] { sequenceResolver });
            var factory = new ResultSetResolverFactory(new ServiceLocator());
            var resolver = factory.Instantiate(args);

            Assert.That(resolver, Is.TypeOf<SequenceCombinationResultSetResolver>());
        }

        [Test()]
        public void Execute_OneSequence_Initialize()
        {
            var args = new SequenceCombinationResultSetResolverArgs(new[] {
                new ListSequenceResolver<DateTime>(
                    new ListSequenceResolverArgs(new[] {
                        new LiteralScalarResolver<string>("2015-01-01"),
                        new LiteralScalarResolver<string>("2016-01-01"),
                        new LiteralScalarResolver<string>("2017-01-01"),
                    })
                )
            });
            var resolver = new SequenceCombinationResultSetResolver(args);
            var rs = resolver.Execute();

            Assert.That(rs.ColumnCount, Is.EqualTo(1));
            Assert.That(rs.Rows.Count, Is.EqualTo(3));
            Assert.That(rs[0][0], Is.EqualTo(new DateTime(2015, 1, 1)));
            Assert.That(rs[1][0], Is.EqualTo(new DateTime(2016, 1, 1)));
            Assert.That(rs[2][0], Is.EqualTo(new DateTime(2017, 1, 1)));
        }

        [Test()]
        public void Execute_TwoSequences_CartesianProduct()
        {
            var resolver = new SequenceCombinationResultSetResolver(
                new SequenceCombinationResultSetResolverArgs([
                    new ListSequenceResolver<DateTime>(
                        new ListSequenceResolverArgs(new[] {
                            new LiteralScalarResolver<string>("2015-01-01"),
                            new LiteralScalarResolver<string>("2016-01-01"),
                            new LiteralScalarResolver<string>("2017-01-01"),
                        })
                    ),
                    new ListSequenceResolver<string>(
                        new ListSequenceResolverArgs(new[] {
                            new LiteralScalarResolver<string>("fr"),
                            new LiteralScalarResolver<string>("be"),
                        })
                    ),
                ])
            );
            var rs = resolver.Execute();

            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs.Rows.Count, Is.EqualTo(6));
            Assert.That(rs[0][0], Is.EqualTo(new DateTime(2015, 1, 1)));
            Assert.That(rs[1][0], Is.EqualTo(new DateTime(2016, 1, 1)));
            Assert.That(rs[2][0], Is.EqualTo(new DateTime(2017, 1, 1)));
            Assert.That(rs[3][0], Is.EqualTo(new DateTime(2015, 1, 1)));
            Assert.That(rs[4][0], Is.EqualTo(new DateTime(2016, 1, 1)));
            Assert.That(rs[5][0], Is.EqualTo(new DateTime(2017, 1, 1)));
            Assert.That(rs[0][1], Is.EqualTo("fr"));
            Assert.That(rs[1][1], Is.EqualTo("fr"));
            Assert.That(rs[2][1], Is.EqualTo("fr"));
            Assert.That(rs[3][1], Is.EqualTo("be"));
            Assert.That(rs[4][1], Is.EqualTo("be"));
            Assert.That(rs[5][1], Is.EqualTo("be"));
        }

        [Test()]
        public void Execute_ThreeSequences_CartesianProduct()
        {
            var resolver = new SequenceCombinationResultSetResolver(
                new SequenceCombinationResultSetResolverArgs([
                    new ListSequenceResolver<decimal>(
                        new ListSequenceResolverArgs(new[] {
                            new LiteralScalarResolver<decimal>(1),
                            new LiteralScalarResolver<decimal>(2),
                        })
                    ),
                    new ListSequenceResolver<DateTime>(
                        new ListSequenceResolverArgs(new[] {
                            new LiteralScalarResolver<string>("2015-01-01"),
                            new LiteralScalarResolver<string>("2016-01-01"),
                            new LiteralScalarResolver<string>("2017-01-01"),
                        })
                    ),
                    new ListSequenceResolver<string>(
                        new ListSequenceResolverArgs(new[] {
                            new LiteralScalarResolver<string>("fr"),
                            new LiteralScalarResolver<string>("be"),
                        })
                    ),
                ])
            );
            var rs = resolver.Execute();

            Assert.That(rs.ColumnCount, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(12));
            Assert.That(rs.Rows.Count(x => x.Field<decimal>(0) == 1), Is.EqualTo(6));
            Assert.That(rs.Rows.Count(x => x.Field<decimal>(0) == 2), Is.EqualTo(6));
            Assert.That(rs.Rows.Count(x => x.Field<DateTime>(1).Year == 2015), Is.EqualTo(4));
            Assert.That(rs.Rows.Count(x => x.Field<DateTime>(1).Year == 2016), Is.EqualTo(4));
            Assert.That(rs.Rows.Count(x => x.Field<DateTime>(1).Year == 2017), Is.EqualTo(4));
            Assert.That(rs.Rows.Count(x => x.Field<string>(2) == "fr"), Is.EqualTo(6));
            Assert.That(rs.Rows.Count(x => x.Field<string>(2) == "be"), Is.EqualTo(6));
        }

        [Test()]
        public void Execute_UniqueSequence_CorrectResult()
        {
            var resolver = new SequenceCombinationResultSetResolver(
                new SequenceResultSetResolverArgs(
                    new ListSequenceResolver<decimal>(
                        new ListSequenceResolverArgs(new[] {
                            new LiteralScalarResolver<decimal>(1),
                            new LiteralScalarResolver<decimal>(2),
                        })
                    )
                )
            );
            var rs = resolver.Execute();

            Assert.That(rs.ColumnCount, Is.EqualTo(1));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
            Assert.That(rs.Rows.Count(x => x.Field<decimal>(0) == 1), Is.EqualTo(1));
            Assert.That(rs.Rows.Count(x => x.Field<decimal>(0) == 2), Is.EqualTo(1));
        }
    }
}