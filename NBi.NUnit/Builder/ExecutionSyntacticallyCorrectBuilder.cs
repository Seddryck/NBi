using System;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionSyntacticallyCorrectBuilder: AbstractExecutionBuilder
    {
        protected SyntacticallyCorrectXml ConstraintXml {get; set;}

        public ExecutionSyntacticallyCorrectBuilder()
        {

        }


        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SyntacticallyCorrectXml))
                throw new ArgumentException("Constraint must be a 'SyntacticallyCorrectXml'");

            ConstraintXml = (SyntacticallyCorrectXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(SyntacticallyCorrectXml syntacticallyCorrectXml)
        {
            var ctr = new SyntacticallyCorrectConstraint();
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
