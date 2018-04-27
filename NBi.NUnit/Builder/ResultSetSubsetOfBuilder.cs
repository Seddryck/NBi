using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using NBi.NUnit.ResultSetComparison;
using System.Data;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class ResultSetSubsetOfBuilder : ResultSetEqualToBuilder
    {
        public ResultSetSubsetOfBuilder()
        {

        }
        protected override EquivalenceKind EquivalenceKind
        {
            get { return EquivalenceKind.SubsetOf; }
        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SubsetOfXml))
                throw new ArgumentException("Constraint must be a 'SubsetOfXml'");

            ConstraintXml = (SubsetOfXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }


        protected override BaseResultSetComparisonConstraint InstantiateConstraint(object obj, SettingsXml settings, TransformationProvider transformation)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder(ServiceLocator);
            argsBuilder.Setup(obj);
            argsBuilder.Setup(settings);
            argsBuilder.Build();

            var factory = ServiceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());

            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            if (transformation != null)
                builder.Setup(transformation.Transform);
            var service = builder.GetService();

            return new SubsetOfConstraint(service);
        }
        
    }
}
