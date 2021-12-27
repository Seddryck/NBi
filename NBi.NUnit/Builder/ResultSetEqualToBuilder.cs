using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.NUnit.ResultSetComparison;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Settings;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.Builder
{
    class ResultSetEqualToBuilder : AbstractResultSetBuilder
    {
        protected EqualToXml ConstraintXml { get; set; }

        protected virtual EquivalenceKind EquivalenceKind
            { get => EquivalenceKind.EqualTo; }

        public ResultSetEqualToBuilder()
        { }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is EqualToXml))
                throw new ArgumentException("Constraint must be a 'EqualToXml'");

            ConstraintXml = (EqualToXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected NBiConstraint InstantiateConstraint()
        {
            BaseResultSetComparisonConstraint ctr = null;

            //Manage transformations
            var transformationProvider = new TransformationProvider(ServiceLocator, new Context(Variables));
            foreach (var columnDef in ConstraintXml.ColumnsDef)
            {
                if (columnDef.Transformation != null)
                    transformationProvider.Add(columnDef.Identifier, columnDef.Transformation);
            }

            if (ConstraintXml.BaseItem is QueryXml xml)
                ctr = InstantiateConstraint(xml, ConstraintXml.Settings, transformationProvider);
            else if (ConstraintXml.ResultSetOld != null)
                ctr = InstantiateConstraint(ConstraintXml.ResultSetOld, ConstraintXml.Settings, transformationProvider);
            else if (ConstraintXml.ResultSet != null)
                ctr = InstantiateConstraint(ConstraintXml.ResultSet, ConstraintXml.Settings, transformationProvider);
            else if (ConstraintXml.XmlSource != null)
                ctr = InstantiateConstraint(ConstraintXml.XmlSource, ConstraintXml.Settings, transformationProvider);

            if (ctr == null)
                throw new ArgumentException();

            //Manage settings for comparaison
            var builder = new SettingsEquivalerBuilder();
            if (ConstraintXml.Behavior == EqualToXml.ComparisonBehavior.SingleRow)
            {
                builder.Setup(false);
                builder.Setup(ConstraintXml.ValuesDefaultType, ConstraintXml.Tolerance);
                builder.Setup(ConstraintXml.ColumnsDef);
            }
            else
            {
                builder.Setup(ConstraintXml.KeysDef, ConstraintXml.ValuesDef);
                builder.Setup(
                    ConstraintXml.KeyName?.Replace(" ", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct(),
                    ConstraintXml.ValueName?.Replace(" ", "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct());
                builder.Setup(ConstraintXml.ValuesDefaultType, ConstraintXml.Tolerance);
                builder.Setup(ConstraintXml.ColumnsDef);
            }

            builder.Build();
            var settings = builder.GetSettings();

            var factory = new EquivalerFactory();
            var comparer = factory.Instantiate(settings, EquivalenceKind);
            ctr = ctr.Using(comparer);
            ctr = ctr.Using(settings);

            //Manage parallelism
            if (ConstraintXml.ParallelizeQueries)
                ctr = ctr.Parallel();
            else
                ctr = ctr.Sequential();

            return ctr;
        }

        protected virtual BaseResultSetComparisonConstraint InstantiateConstraint(ResultSetSystemXml xml, SettingsXml settings)
        {
            xml.Settings = settings;
            var helper = new ResultSetSystemHelper(ServiceLocator, SettingsXml.DefaultScope.Assert, Variables);
            var resolver = helper.InstantiateResolver(xml);

            return InstantiateConstraint(resolver);
        }

        protected virtual BaseResultSetComparisonConstraint InstantiateConstraint(object obj, SettingsXml settings, TransformationProvider transformation)
        {
            var argsBuilder = new ResultSetResolverArgsBuilder(ServiceLocator);
            argsBuilder.Setup(obj, settings, SettingsXml.DefaultScope.Assert, Variables);
            argsBuilder.Setup(transformation);
            argsBuilder.Build();

            var factory = ServiceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(argsBuilder.GetArgs());

            return InstantiateConstraint(resolver);
        }

        protected virtual BaseResultSetComparisonConstraint InstantiateConstraint(IResultSetResolver resolver)
            => new EqualToConstraint(resolver);
    }
}
