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
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Core.Scalar.Comparer;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    class ResultSetLookupMatchesBuilder : AbstractResultSetBuilder
    {
        protected LookupMatchesXml ConstraintXml {get; set;}

        public ResultSetLookupMatchesBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is LookupMatchesXml))
                throw new ArgumentException("Constraint must be a 'lookup-matches'");

            ConstraintXml = (LookupMatchesXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            var ctrXml = ConstraintXml as LookupMatchesXml;
            ctrXml.ResultSet.Settings = ctrXml.Settings;

            
            var joinMappings = new ColumnMappingCollection(BuildMappings(ctrXml.Join));
            var inclusionMappings = new ColumnMappingCollection(BuildMappings(ctrXml.Inclusion));
            var inclusionTolerances = BuildTolerances(ctrXml.Inclusion);

            var builder = new ResultSetServiceBuilder();
            var helper = new ResultSetSystemHelper(ServiceLocator, SettingsXml.DefaultScope.Assert, Variables);
            builder.Setup(helper.InstantiateResolver(ctrXml.ResultSet));
            builder.Setup(helper.InstantiateAlterations(ctrXml.ResultSet));
            var service = builder.GetService();

            var ctr = new LookupMatchesConstraint(service);
            Constraint = ctr.Using(joinMappings, inclusionMappings, inclusionTolerances);
        }

        private IEnumerable<ColumnMapping> BuildMappings(JoinXml joinXml)
        {
            var factory = new ColumnIdentifierFactory();

            return joinXml?.Mappings.Select(mapping => new ColumnMapping(
                        factory.Instantiate(mapping.Candidate)
                        , factory.Instantiate(mapping.Reference)
                        , mapping.Type))
                .Union(
                    joinXml?.Usings.Select(@using => new ColumnMapping(
                        factory.Instantiate(@using.Column)
                        , @using.Type)
                    ));
        }

        private IDictionary<IColumnIdentifier, Tolerance> BuildTolerances(InclusionXml inclusionXml)
        {
            var columnIdentifierFactory = new ColumnIdentifierFactory();
            var toleranceFactory = new ToleranceFactory();

            var tuples =  inclusionXml?.Mappings.Where(mapping => !string.IsNullOrEmpty(mapping.Tolerance))
                        .Select(mapping => new
                        {
                            Identifier = columnIdentifierFactory.Instantiate(mapping.Candidate),
                            Tolerance = toleranceFactory.Instantiate(mapping.Type, mapping.Tolerance)
                        }
                    )
                .Union(
                    inclusionXml?.Usings.Where(@using => !string.IsNullOrEmpty(@using.Tolerance))
                        .Select(@using => new
                        {
                            Identifier = columnIdentifierFactory.Instantiate(@using.Column),
                            Tolerance = toleranceFactory.Instantiate(@using.Type, @using.Tolerance)
                        }
                    ));

            var dico = new Dictionary<IColumnIdentifier, Tolerance>();
            foreach (var tuple in tuples)
                dico.Add(tuple.Identifier, tuple.Tolerance);

            return dico;
        }
    }
}
