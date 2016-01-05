using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.NUnit.Execution;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;

namespace NBi.NUnit.Builder
{
    class ExecutionRowCountBuilder : AbstractExecutionBuilder
    {
        protected RowCountXml ConstraintXml {get; set;}

        public ExecutionRowCountBuilder()
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
            var childConstraint = BuildChildConstraint(ConstraintXml.Comparer);
            var ctr = new RowCountConstraint(childConstraint);

            if (ConstraintXml.Filter != null)
            {
                var filterXml = ConstraintXml.Filter;
                var expressions = new List<IColumnExpression>();
                if (filterXml.Expression!=null)
                     expressions .Add(filterXml.Expression);
                var filter = new PredicateFilter
                                (
                                    filterXml.Variables
                                    , expressions
                                    , filterXml.Predicate
                                );
                ctr = ctr.With(filter);
            }

            return ctr;
        }

        protected virtual NUnitCtr.Constraint BuildChildConstraint(AbstractComparerXml xml)
        {
            var value = 0;
            try
            {
                value = Int32.Parse(xml.Value);
            }
            catch (Exception ex)
            {
                var exception = new ArgumentException
                                    (
                                        String.Format("The assertion row-count is expecting an integer value for comparison. The provided value '{0}' is not a integer value.", value)
                                        , ex
                                    );
                throw exception;
            }
             

            NUnitCtr.Constraint ctr = null;
            if (xml is LessThanXml)
            {
                if (((LessThanXml)xml).OrEqual)
                    ctr = new NUnitCtr.LessThanOrEqualConstraint(value);
                else
                    ctr = new NUnitCtr.LessThanConstraint(value);
            }
            else if (xml is MoreThanXml)
            {
                if (((MoreThanXml)xml).OrEqual)
                    ctr = new NUnitCtr.GreaterThanOrEqualConstraint(value);
                else
                    ctr = new NUnitCtr.GreaterThanConstraint(value);
            }
            else if (xml is EqualXml)
            {
                ctr = new NUnitCtr.EqualConstraint(value);
            }

            if (ctr == null)
                throw new ArgumentException();

            return ctr;
        }

    }
}
