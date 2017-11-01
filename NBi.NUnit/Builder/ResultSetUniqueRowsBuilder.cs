using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;


namespace NBi.NUnit.Builder
{
    class ResultSetUniqueRowsBuilder : AbstractResultSetBuilder
    {
        protected UniqueRowsXml ConstraintXml {get; set;}

        public ResultSetUniqueRowsBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is UniqueRowsXml))
                throw new ArgumentException("Constraint must be a 'NoDuplicateXml'");

            ConstraintXml = (UniqueRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint()
        {           
            var ctr = new UniqueRowsConstraint();
            return ctr;
        }

    }
}
