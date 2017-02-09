using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.Xml;
using NBi.Core.Transformation;

namespace NBi.NUnit.Builder
{
    class ExecutionEqualToBuilder : AbstractExecutionBuilder
    {
        protected EqualToXml ConstraintXml { get; set; }

        public ExecutionEqualToBuilder()
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
            EqualToConstraint ctr = null;

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
                ctr = new EqualToConstraint(cmd);

            }
            else if (ConstraintXml.ResultSet != null)
            {
                if (!string.IsNullOrEmpty(ConstraintXml.ResultSet.File))
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in external file!");
                    ctr = new EqualToConstraint(ConstraintXml.ResultSet.GetFile());
                    if (ConstraintXml.Settings.CsvProfile != null)
                        ctr = ctr.CsvProfile(ConstraintXml.Settings.CsvProfile);
                }
                else if (ConstraintXml.ResultSet.Rows != null)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "ResultSet defined in embedded resultSet!");
                    ctr = new EqualToConstraint(ConstraintXml.ResultSet.Content);
                }
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
                }
                else if (ConstraintXml.XmlSource.Url != null)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Xml file at '{0}'", ConstraintXml.XmlSource.Url.Value));
                    engine = new XPathUrlEngine(ConstraintXml.XmlSource.Url.Value, ConstraintXml.XmlSource.XPath.From.Value, selects);
                }
                else
                    throw new ArgumentException("File or Url can't be both empty when declaring an xml-source.");

                ctr = new EqualToConstraint(engine);
            }

            if (ctr == null)
                throw new ArgumentException();

            //Manage settings for comparaison
            var builder = new ResultSetComparisonBuilder();
            if (ConstraintXml.Behavior == EqualToXml.ComparisonBehavior.SingleRow)
            {
                
                builder.Setup(false, 0, null, 0, null,
                    ConstraintXml.ValuesDefaultType,
                    new NumericToleranceFactory().Instantiate(ConstraintXml.Tolerance),
                    ConstraintXml.ColumnsDef
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
                    new NumericToleranceFactory().Instantiate(ConstraintXml.Tolerance),
                    ConstraintXml.ColumnsDef
                );
            }

            builder.Build();
            ctr = ctr.Using(builder.GetComparer());

            var settings = builder.GetSettings();
            ctr = ctr.Using(settings);

            //Manage transformations
            var transformationProvider = new TransformationProvider();
            foreach (var columnDef in ConstraintXml.ColumnsDef)
            {
                if (columnDef.Transformation != null)
                    transformationProvider.Add(columnDef.Index, columnDef.Transformation);
            }

            ctr = ctr.Using(transformationProvider);

            //Manage parallelism
            if (ConstraintXml.ParallelizeQueries)
                ctr = ctr.Parallel();
            else
                ctr = ctr.Sequential();

            //Manage persistance
            //EqualToConstraint.PersistanceItems persi = 0;
            //if (ConstraintXml.GetCommand() != null)
            //    persi += (int)EqualToConstraint.PersistanceItems.actual;
            //if (SystemUnderTestXml is QueryXml)
            //    persi += (int)EqualToConstraint.PersistanceItems.expected;
            //if (!(persi==0 || ConstraintXml.Query==null || string.IsNullOrEmpty(ConstraintXml.Test.Name)))
            //    ctr.Persist(ConstraintXml.Persistance, persi, ConstraintXml.Test.Name);

            return ctr;
        }

    }
}
