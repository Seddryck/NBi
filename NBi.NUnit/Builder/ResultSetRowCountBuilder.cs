using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Scalar;
using NBi.Core.Calculation.Predicate;

namespace NBi.NUnit.Builder
{
    class ResultSetRowCountBuilder : AbstractResultSetBuilder
    {
        protected RowCountXml ConstraintXml { get; set; }

        public ResultSetRowCountBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is RowCountXml))
                throw new ArgumentException("Constraint must be a 'RowCountXml'");

            ConstraintXml = (RowCountXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint()
        {
            RowCountConstraint ctr;
            var comparer = ConstraintXml.Comparer as ScalarReferencePredicateXml;
            var childConstraint = BuildChildConstraint(comparer);

            IResultSetFilter filter = null;
            if (ConstraintXml.Filter != null)
            {
                var filterXml = ConstraintXml.Filter;
                var expressions = new List<IColumnExpression>();
                if (filterXml.Expression != null)
                    expressions.Add(filterXml.Expression);

                var value = EvaluatePotentialVariable(comparer.Reference.ToString().Replace(" ", ""));

                var factory = new ResultSetFilterFactory(Variables);
                if (filterXml.Predication != null)
                {
                    var helper = new PredicateArgsBuilder(ServiceLocator, Variables);
                    var args = helper.Execute(filterXml.Predication.ColumnType, filterXml.Predication.Predicate);

                    filter = factory.Instantiate
                                (
                                    filterXml.Aliases
                                    , expressions
                                    , new PredicationArgs(filterXml.Predication.Operand, args)
                                );
                }

                else if (filterXml.Combination != null)
                {
                    var helper = new PredicateArgsBuilder(ServiceLocator, Variables);
                    var predicationArgs = new List<PredicationArgs>();
                    foreach (var predication in filterXml.Combination.Predications)
                    {
                        var args = helper.Execute(predication.ColumnType, predication.Predicate);
                        predicationArgs.Add(new PredicationArgs(predication.Operand, args));
                    }

                    filter = factory.Instantiate
                                (
                                    filterXml.Aliases
                                    , expressions
                                    , filterXml.Combination.Operator
                                    , predicationArgs
                                );
                }
                if ((value is string & (value as string).EndsWith("%")))
                    ctr = new RowCountFilterPercentageConstraint(childConstraint, filter);
                else
                    ctr = new RowCountFilterConstraint(childConstraint, filter);
            }
            else
                ctr = new RowCountConstraint(childConstraint);

            return ctr;
        }

        protected virtual DifferedConstraint BuildChildConstraint(ScalarReferencePredicateXml xml)
        {
            var builder = new ScalarResolverArgsBuilder(ServiceLocator);

            if (!string.IsNullOrEmpty(xml.Reference))
            {
                if (xml.Reference.Trim().EndsWith("%"))
                    builder.Setup(xml.Reference.Trim().Substring(0, xml.Reference.Trim().IndexOf("%")));
                else
                    builder.Setup(xml.Reference);
            }

            if (xml.QueryScalar != null)
                builder.Setup(xml.QueryScalar);

            if (xml.Projection != null)
                builder.Setup(xml.Projection);

            builder.Setup(ConstraintXml.Settings);
            builder.Setup(Variables);
            builder.Build();
            var args = builder.GetArgs();

            var factory = ServiceLocator.GetScalarResolverFactory();
            var resolver = factory.Instantiate<decimal>(args);

            Type ctrType = null;
            if (xml is LessThanXml)
            {
                if (((LessThanXml)xml).OrEqual)
                    ctrType = typeof(NUnitCtr.LessThanOrEqualConstraint);
                else
                    ctrType = typeof(NUnitCtr.LessThanConstraint);
            }
            else if (xml is MoreThanXml)
            {
                if (((MoreThanXml)xml).OrEqual)
                    ctrType = typeof(NUnitCtr.GreaterThanOrEqualConstraint);
                else
                    ctrType = typeof(NUnitCtr.GreaterThanConstraint);
            }
            else if (xml is EqualXml)
                ctrType = typeof(NUnitCtr.EqualConstraint);

            if (ctrType == null)
                throw new ArgumentException();

            return new DifferedConstraint(ctrType, resolver);
        }

    }
}
