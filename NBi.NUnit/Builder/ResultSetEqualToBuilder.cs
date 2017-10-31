using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.ResultSetComparison;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using System.Data;
using NBi.Core.ResultSet.Loading;
using System.IO;
using NBi.Core.ResultSet.Resolver.Query;

namespace NBi.NUnit.Builder
{
    class ResultSetEqualToBuilder : AbstractResultSetBuilder
    {
        protected EqualToXml ConstraintXml { get; set; }

        protected virtual ComparisonKind ComparisonKind
        {
            get { return ComparisonKind.EqualTo; }
        }

        public ResultSetEqualToBuilder()
        {

        }

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
            var transformationProvider = new TransformationProvider();
            foreach (var columnDef in ConstraintXml.ColumnsDef)
            {
                if (columnDef.Transformation != null)
                    transformationProvider.Add(columnDef.Index, columnDef.Transformation);
            }

            if (ConstraintXml.GetCommand() != null)
            {
                var commandText = ConstraintXml.GetCommand().CommandText;
                var connectionString = ConstraintXml.GetCommand().Connection.ConnectionString;
                var timeout = ((QueryXml)(ConstraintXml.BaseItem)).Timeout;
                IEnumerable<IQueryParameter> parameters = null;
                IEnumerable<IQueryTemplateVariable> variables = null;
                if (ConstraintXml.Query != null)
                {
                    parameters = ConstraintXml.Query.GetParameters();
                    variables = ConstraintXml.Query.GetVariables();
                }

                var commandBuilder = new CommandBuilder();
                var cmd = commandBuilder.Build(connectionString, commandText, parameters, variables, timeout);

                var args = new DbCommandQueryResolverArgs(cmd);

                ctr = InstantiateConstraint(args, transformationProvider);
            }
            else if (ConstraintXml.ResultSet != null)
            {
                if (!string.IsNullOrEmpty(ConstraintXml.ResultSet.File))
                {
                    var file = string.Empty;
                    if (Path.IsPathRooted(ConstraintXml.ResultSet.File))
                        file = ConstraintXml.ResultSet.File;
                    else
                        file = ConstraintXml.Settings?.BasePath + ConstraintXml.ResultSet.File;

                    ctr = InstantiateConstraint(file, transformationProvider);
                }
                else
                    ctr = InstantiateConstraint(ConstraintXml.ResultSet.Content, transformationProvider);

            }
            else if (ConstraintXml.XmlSource != null)
            {
                var selects = new List<AbstractSelect>();
                var factory = new SelectFactory();
                foreach (var select in ConstraintXml.XmlSource.XPath.Selects)
                    selects.Add(factory.Instantiate(select.Value, select.Attribute, select.Evaluate));

                XPathEngine engine = null;
                if (ConstraintXml.XmlSource.File != null)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Xml file at '{0}'", ConstraintXml.XmlSource.GetFile()));
                    engine = new XPathFileEngine(ConstraintXml.XmlSource.GetFile(), ConstraintXml.XmlSource.XPath.From.Value, selects);
                    ctr = InstantiateConstraint(engine, transformationProvider);
                }
                else if (ConstraintXml.XmlSource.Url != null)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Xml file at '{0}'", ConstraintXml.XmlSource.Url.Value));
                    engine = new XPathUrlEngine(ConstraintXml.XmlSource.Url.Value, ConstraintXml.XmlSource.XPath.From.Value, selects);
                    ctr = InstantiateConstraint(engine, transformationProvider);
                }
                else
                    throw new ArgumentException("File or Url can't be both empty when declaring an xml-source.");
                
                
            }

            if (ctr == null)
                throw new ArgumentException();

            //Manage settings for comparaison
            var builder = new ResultSetComparisonBuilder();
            if (ConstraintXml.Behavior == EqualToXml.ComparisonBehavior.SingleRow)
            {

                builder.Setup(false, 0, null, 0, null,
                    ConstraintXml.ValuesDefaultType,
                    ToleranceFactory.Instantiate(ConstraintXml.ValuesDefaultType, ConstraintXml.Tolerance),
                    ConstraintXml.ColumnsDef
                    , ComparisonKind.EqualTo
                );

            }
            else
            {
                builder.Setup(
                    true,
                    ConstraintXml.KeysDef,
                    ConstraintXml.KeyName,
                    ConstraintXml.ValuesDef,
                    ConstraintXml.ValueName,
                    ConstraintXml.ValuesDefaultType,
                    ToleranceFactory.Instantiate(ConstraintXml.ValuesDefaultType, ConstraintXml.Tolerance),
                    ConstraintXml.ColumnsDef,
                    ComparisonKind
                );
            }

            builder.Build();
            ctr = ctr.Using(builder.GetComparer());

            var settings = builder.GetSettings();
            ctr = ctr.Using(settings);

            

            //Manage parallelism
            if (ConstraintXml.ParallelizeQueries)
                ctr = ctr.Parallel();
            else
                ctr = ctr.Sequential();

            return ctr;
        }

        protected virtual BaseResultSetComparisonConstraint InstantiateConstraint(object obj, TransformationProvider transformation)
        {
            var factory = new ResultSetLoaderFactory();
            factory.Using(ConstraintXml.Settings?.CsvProfile);
            var loader = factory.Instantiate(obj);

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            if (transformation != null)
                builder.Setup(transformation.Transform);

            var service = builder.GetService();

            return new EqualToConstraint(service);
        }


    }
}
