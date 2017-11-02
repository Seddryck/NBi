using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;

namespace NBi.NUnit.Builder
{
    class ResultSetNoRowsBuilder : AbstractResultSetBuilder
    {
        protected NoRowsXml ConstraintXml {get; set;}
        
        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is NoRowsXml))
                throw new ArgumentException("Constraint must be a 'NoRowXml'");

            ConstraintXml = (NoRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected virtual NBiConstraint InstantiateConstraint()
        {
            var filter = InstantiateFilter();
            var ctr = new NoRowsConstraint(filter);
            return ctr;
        }

        protected BasePredicateFilter InstantiateFilter()
        {
            var expressions = new List<IColumnExpression>();
            if (ConstraintXml.Expression != null)
                expressions.Add(ConstraintXml.Expression);

            var factory = new PredicateFilterFactory();
            if (ConstraintXml.Predicate != null)
            {
                if (ConstraintXml.Predicate.Reference != null)
                    ConstraintXml.Predicate.Reference = EvaluatePotentialVariable(ConstraintXml.Predicate.Reference);


                return factory.Instantiate
                            (
                                ConstraintXml.Aliases
                                , expressions
                                , ConstraintXml.Predicate
                            );
            }
            else if (ConstraintXml.Combination != null)
            {
                var predicateInfos = new List<IPredicateInfo>();
                foreach (var predicateXml in ConstraintXml.Combination.Predicates)
                {
                    if (predicateXml.Reference != null)
                        predicateXml.Reference = EvaluatePotentialVariable(predicateXml.Reference);

                    predicateInfos.Add(predicateXml);
                }

                return factory.Instantiate
                            (
                                ConstraintXml.Aliases
                                , expressions
                                , ConstraintXml.Combination.Operator
                                , predicateInfos
                            );
            }
            else
                throw new ArgumentException("You must specify a predicate or a combination of predicates. None of them is specified");
        }
    }
}
