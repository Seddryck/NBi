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
            RowCountConstraint ctr;
            var childConstraint = BuildChildConstraint(ConstraintXml.Comparer);

            IResultSetFilter filter = null;
            if (ConstraintXml.Filter != null)
            {
                var filterXml = ConstraintXml.Filter;
                var expressions = new List<IColumnExpression>();
                if (filterXml.Expression!=null)
                     expressions .Add(filterXml.Expression);
                 filter = new PredicateFilter
                                (
                                    filterXml.Variables
                                    , expressions
                                    , filterXml.Predicate
                                );
            }

            if (ConstraintXml.Comparer.Value.Replace(" ", "").EndsWith("%"))
                ctr = new RowCountPercentageConstraint(childConstraint, filter);
            else
            {
                ctr = new RowCountConstraint(childConstraint);
                if (filter != null)
                    ctr = ctr.With(filter);
            }

            return ctr;
        }

        protected virtual NUnitCtr.Constraint BuildChildConstraint(AbstractComparerXml xml)
        {
            
            var value = xml.Value.Replace(" ","");

            Decimal numericValue;
            try
            {
                if (value.EndsWith("%"))
                    numericValue = Decimal.Parse(xml.Value.Substring(0, xml.Value.Length-1)) / new Decimal(100);
                else
                    numericValue = Int32.Parse(xml.Value);
            }
            catch (Exception ex)
            {
                var exception = new ArgumentException
                                    (
                                        String.Format("The assertion row-count is expecting an integer or percentage value for comparison. The provided value '{0}' is not a integer or percentage value.", value)
                                        , ex
                                    );
                throw exception;
            }
             

            NUnitCtr.Constraint ctr = null;
            if (xml is LessThanXml)
            {
                if (((LessThanXml)xml).OrEqual)
                    ctr = new NUnitCtr.LessThanOrEqualConstraint(numericValue);
                else
                    ctr = new NUnitCtr.LessThanConstraint(numericValue);
            }
            else if (xml is MoreThanXml)
            {
                if (((MoreThanXml)xml).OrEqual)
                    ctr = new NUnitCtr.GreaterThanOrEqualConstraint(numericValue);
                else
                    ctr = new NUnitCtr.GreaterThanConstraint(numericValue);
            }
            else if (xml is EqualXml)
            {
                ctr = new NUnitCtr.EqualConstraint(numericValue);
            }

            if (ctr == null)
                throw new ArgumentException();

            return ctr;
        }

    }
}
