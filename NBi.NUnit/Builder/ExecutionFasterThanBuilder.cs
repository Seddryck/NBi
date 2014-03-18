using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionFasterThanBuilder: AbstractExecutionBuilder
    {
        protected FasterThanXml ConstraintXml {get; set;}

        public ExecutionFasterThanBuilder()
        {

        }


        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is FasterThanXml))
                throw new ArgumentException("Constraint must be a 'FasterThanXml'");

            ConstraintXml = (FasterThanXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(FasterThanXml fasterThanXml)
        {
            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(fasterThanXml.MaxTimeMilliSeconds);
            if (fasterThanXml.CleanCache)
                ctr = ctr.CleanCache();
            if (fasterThanXml.TimeOutMilliSeconds > 0)
                ctr = ctr.TimeOutMilliSeconds(fasterThanXml.TimeOutMilliSeconds);
            return ctr;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected override IDbCommand InstantiateSystemUnderTest(ExecutionXml xml)
        {
            if (xml.Item is QueryableXml)
            { 
                var commandText = xml.Item.GetQuery();
                var connectionString = xml.Item.GetConnectionString();
                var parameters = xml.Item.GetParameters();
                var variables = xml.Item.GetVariables();

                var commandBuilder = new CommandBuilder();
                var cmd = commandBuilder.Build(connectionString, commandText, parameters, variables);
                return cmd;
            }
            throw new ArgumentException();
        }

    }
}
