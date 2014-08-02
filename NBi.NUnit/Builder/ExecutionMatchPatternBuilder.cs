using System;
using System.Data;
using System.Linq;
using NBi.Core.Format;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
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
            var regexBuilder = new RegexBuilder();

            var ctr = new MatchPatternConstraint();
            if (!string.IsNullOrEmpty(matchPatternXml.Regex))
                ctr = ctr.Regex(matchPatternXml.Regex);

            if (matchPatternXml.NumericFormat != null && !matchPatternXml.NumericFormat.IsEmpty)
                ctr = ctr.Regex(regexBuilder.Build(matchPatternXml.NumericFormat));

            if (matchPatternXml.CurrencyFormat != null && !matchPatternXml.CurrencyFormat.IsEmpty)
                ctr = ctr.Regex(regexBuilder.Build(matchPatternXml.CurrencyFormat));
            
            return ctr;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected override IDbCommand InstantiateSystemUnderTest(ExecutionXml xml)
        {
            if (xml.Item is QueryableXml)
            {
                var commandText = (xml.Item as QueryableXml).GetQuery();
                var connectionString = xml.Item.GetConnectionString();
                var parameters = (xml.Item as QueryableXml).GetParameters();
                var variables = (xml.Item as QueryableXml).GetVariables();

                var commandBuilder = new CommandBuilder();
                var cmd = commandBuilder.Build(connectionString, commandText, parameters, variables);
                return cmd;
            }
            throw new ArgumentException();
        }

    }
}
