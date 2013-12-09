using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionMatchPatternBuilder: AbstractExecutionBuilder
    {
        protected MatchPatternXml ConstraintXml {get; set;}

        public ExecutionMatchPatternBuilder()
        {

        }


        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is MatchPatternXml))
                throw new ArgumentException("Constraint must be a 'MatchPatternXml'");

            ConstraintXml = (MatchPatternXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(MatchPatternXml matchPatternXml)
        {
            var ctr = new MatchPatternConstraint();
            if (!string.IsNullOrEmpty(matchPatternXml.Regex))
                ctr = ctr.Regex(matchPatternXml.Regex);
            
            return ctr;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected override IDbCommand InstantiateSystemUnderTest(ExecutionXml queryXml)
        {
            var conn = new ConnectionFactory().Get(queryXml.Item.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = queryXml.Item.GetQuery();

            return cmd;
        }

    }
}
