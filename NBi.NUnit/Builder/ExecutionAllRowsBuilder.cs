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
    class ExecutionAllRowsBuilder : AbstractExecutionBuilder
    {
        protected AllRowsXml ConstraintXml {get; set;}

        public ExecutionAllRowsBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is AllRowsXml))
                throw new ArgumentException("Constraint must be a 'RowCountXml'");

            ConstraintXml = (AllRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint()
        {
            AllRowsConstraint ctr;
            
            var expressions = new List<IColumnExpression>();
            if (ConstraintXml.Expression!=null)
                expressions.Add(ConstraintXml.Expression);

            if (ConstraintXml.Predicate.Reference != null)
                ConstraintXml.Predicate.Reference = EvaluatePotentialVariable(ConstraintXml.Predicate.Reference);

            var factory = new PredicateFilterFactory();
            var filter = factory.Instantiate
                        (
                            ConstraintXml.Aliases
                            , expressions
                            , ConstraintXml.Predicate
                        );

            ctr = new AllRowsConstraint(filter);
            return ctr;
        }

    }
}
