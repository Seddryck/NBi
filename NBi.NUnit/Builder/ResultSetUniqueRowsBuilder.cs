using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.ResultSet.Uniqueness;

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
                throw new ArgumentException("Constraint must be a 'UniqueRowsXml'");

            ConstraintXml = (UniqueRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            var ctrXml = ConstraintXml as UniqueRowsXml;

            var builder = new SettingsEvaluatorBuilder();
            builder.Setup(ctrXml.KeysSet, ctrXml.ValuesSet);
            builder.Setup(ctrXml.Columns);
            builder.Build();

            var ctr = new UniqueRowsConstraint();

            var settings = builder.GetSettings();

            var factory = new EvaluatorFactory();
            var evaluator = factory.Instantiate(settings);
            Constraint = ctr.Using(evaluator);
        }

    }
}
