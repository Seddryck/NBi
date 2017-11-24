using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.ResultSetComparison;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class ResultSetSupersetOfBuilder : ResultSetEqualToBuilder
    {
        protected override EquivalenceKind EquivalenceKind
        {
            get { return EquivalenceKind.SupersetOf; }
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

        protected override BaseResultSetComparisonConstraint InstantiateConstraint(object obj, SettingsXml settings, TransformationProvider transformation)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder();
            argsBuilder.Setup(obj);
            argsBuilder.Build();

            var factory = new ResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());

            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            if (transformation != null)
                builder.Setup(transformation.Transform);
            var service = builder.GetService();

            return new SupersetOfConstraint(service);
        }
    }
}
