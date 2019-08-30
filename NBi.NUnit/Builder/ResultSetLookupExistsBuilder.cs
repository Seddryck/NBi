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
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class ResultSetLookupExistsBuilder : AbstractResultSetBuilder
    {
        protected LookupExistsXml ConstraintXml {get; set;}

        public ResultSetLookupExistsBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is LookupExistsXml))
                throw new ArgumentException("Constraint must be a 'ReferenceExistsXml'");

            ConstraintXml = (LookupExistsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            var ctrXml = ConstraintXml as LookupExistsXml;
            ctrXml.ResultSet.Settings = ctrXml.Settings;

            var factory = new ColumnIdentifierFactory();
            var mappings = new ColumnMappingCollection(
                ctrXml.Join?.Mappings
                    .Select(mapping => new ColumnMapping(
                        factory.Instantiate(mapping.Candidate)
                        , factory.Instantiate(mapping.Reference)
                        , mapping.Type))
                .Union(
                    ctrXml.Join?.Usings.Select(@using => new ColumnMapping(
                        factory.Instantiate(@using.Column)
                        , @using.Type)
                    )));

            var builder = new ResultSetServiceBuilder();
            var helper = new ResultSetSystemHelper(ServiceLocator, SettingsXml.DefaultScope.Assert, Variables);
            builder.Setup(helper.InstantiateResolver(ctrXml.ResultSet));
            builder.Setup(helper.InstantiateAlterations(ctrXml.ResultSet));
            var service = builder.GetService();

            var ctr = ctrXml.IsReversed ? new LookupReverseExistsConstraint(service) : new LookupExistsConstraint(service);
            Constraint = ctr.Using(mappings);
        }

    }
}
