using System;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.Scoring;

namespace NBi.NUnit.Builder
{
    class ScalarScoreBuilder : AbstractScalarBuilder
    {
        protected ScoreXml ConstraintXml { get; set; }

        public ScalarScoreBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ScoreXml))
                throw new ArgumentException("Constraint must be a 'ScoreXml'");

            ConstraintXml = (ScoreXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint() => new ScoreConstraint(ConstraintXml.Threshold);
        
    }
}
