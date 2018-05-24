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

namespace NBi.NUnit.Builder
{
    class ResultSetRowCountBuilder : AbstractResultSetBuilder
    {
        protected RowCountXml ConstraintXml {get; set;}

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
            var childConstraint = BuildChildConstraint(ConstraintXml.Comparer);

            IResultSetFilter filter = null;
            if (ConstraintXml.Filter != null)
            {
                var filterXml = ConstraintXml.Filter;
                var expressions = new List<IColumnExpression>();
                if (filterXml.Expression!=null)
                     expressions.Add(filterXml.Expression);

                var value = EvaluatePotentialVariable(ConstraintXml.Comparer.Value.Replace(" ", ""));

                var factory = new ResultSetFilterFactory();
                if (filterXml.Predication != null)
                    filter = factory.Instantiate
                                (
                                    filterXml.Aliases
                                    , expressions
                                    , filterXml.Predication
                                );
                else if (filterXml.Combination != null)
                    filter = factory.Instantiate
                                (
                                    filterXml.Aliases
                                    , expressions
                                    , filterXml.Combination.Operator
                                    , filterXml.Combination.Predicates
                                );
                if ((value is string & (value as string).EndsWith("%")))
                    ctr = new RowCountFilterPercentageConstraint(childConstraint, filter);
                else
                    ctr = new RowCountFilterConstraint(childConstraint, filter);
            }
            else
                ctr = new RowCountConstraint(childConstraint);

            return ctr;
        }

        protected virtual DifferedConstraint BuildChildConstraint(PredicateXml xml)
        {
            var builder = new ScalarResolverArgsBuilder(ServiceLocator);

            if (!string.IsNullOrEmpty(xml.Value))
            {
                if (xml.Value.Trim().EndsWith("%"))
                    builder.Setup(xml.Value.Trim().Substring(0, xml.Value.Trim().IndexOf("%")));
                else
                    builder.Setup(xml.Value);
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
