using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using NBi.NUnit.ResultSetComparison;
using System.Data;
using NBi.Core.ResultSet.Loading;

namespace NBi.NUnit.Builder
{
    class ResultSetSubsetOfBuilder : ResultSetEqualToBuilder
    {
        public ResultSetSubsetOfBuilder()
        {

        }
        protected override ComparisonKind ComparisonKind
        {
            get { return ComparisonKind.SubsetOf; }
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


        protected override BaseResultSetComparisonConstraint InstantiateConstraint(object obj, TransformationProvider transformation)
        {
            var factory = new ResultSetLoaderFactory();
            factory.Using(ConstraintXml.Settings?.CsvProfile);
            var loader = factory.Instantiate(obj);

            var builder = new ResultSetServiceBuilder() { Loader = loader };
            if (transformation != null)
                builder.AddTransformation(transformation);
            var service = builder.GetService();

            return new SubsetOfConstraint(service);
        }
        
    }
}
