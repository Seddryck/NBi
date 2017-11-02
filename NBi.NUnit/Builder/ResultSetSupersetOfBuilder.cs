using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.ResultSetComparison;
using NBi.Core.ResultSet.Loading;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Comparison;

namespace NBi.NUnit.Builder
{
    class ResultSetSupersetOfBuilder : ResultSetEqualToBuilder
    {
        protected override ComparisonKind ComparisonKind
        {
            get { return ComparisonKind.SupersetOf; }
        }

        public ResultSetSupersetOfBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SupersetOfXml))
                throw new ArgumentException("Constraint must be a 'SupersetOfXml'");

            ConstraintXml = (SupersetOfXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(object obj, TransformationProvider transformation)
        {
            var factory = new ResultSetLoaderFactory();
            factory.Using(ConstraintXml.Settings?.CsvProfile);
            var loader = factory.Instantiate(obj);

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            if (transformation != null)
                builder.Setup(transformation.Transform);
            var service = builder.GetService();

            return new SupersetOfConstraint(service);
        }
    }
}
