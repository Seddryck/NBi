using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Core.ResultSet.Lookup;
using NBi.NUnit.Builder.Helper;
using NBi.Core.ResultSet;
using NBi.NUnit.ResultSetComparison;

namespace NBi.NUnit.Builder
{
    class ResultSetReferenceExistsBuilder : AbstractResultSetBuilder
    {
        protected ReferenceExistsXml ConstraintXml {get; set;}

        public ResultSetReferenceExistsBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is ReferenceExistsXml))
                throw new ArgumentException("Constraint must be a 'ReferenceExistsXml'");

            ConstraintXml = (ReferenceExistsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            var ctrXml = ConstraintXml as ReferenceExistsXml;

            var mappings = new ColumnMappingCollection();
            foreach (var mapping in ctrXml.Mappings)
                mappings.Add(new ColumnMapping(mapping.Child, mapping.Parent, mapping.Type));

            var builder = new ResultSetServiceBuilder();
            builder.Setup(Helper.InstantiateResolver(ctrXml.ResultSet));
            builder.Setup(Helper.InstantiateAlterations(ctrXml.ResultSet));
            var service = builder.GetService();

            var ctr = new ReferenceExistsConstraint(service);
            Constraint = ctr.Using(mappings);
        }

    }
}
