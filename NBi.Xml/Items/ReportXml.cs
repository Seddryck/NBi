using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Report;
using NBi.Xml.Settings;
using NBi.Xml.Constraints;

namespace NBi.Xml.Items
{
    public class ReportXml : ReportBaseXml, IReferenceFriendly
    {

        [XmlAttribute("ref")]
        public string Reference { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("dataset")]
        public string Dataset { get; set; }

        [XmlElement("parameter")]
        public new List<QueryParameterXml> Parameters { get; set; }

        public ReportXml()
        {
            Parameters = new List<QueryParameterXml>();
        }

        public override string GetQuery()
        {
            var parser = ParserFactory.GetParser(
                    Source
                    , Path
                    , Name
                    , Dataset
                );

            var request = ParserFactory.GetRequest(
                    Source
                    , Settings.BasePath
                    , Path
                    , Name
                    , Dataset
                );

            var query = parser.ExtractQuery(request);

            return query;
        }


        public new List<QueryParameterXml> GetParameters()
        {
            var list = Parameters;
            foreach (var param in Default.Parameters)
                if (!Parameters.Exists(p => p.Name == param.Name))
                    list.Add(param);

            return list;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual IDbCommand GetCommand()
        {
            var conn = new ConnectionFactory().Get(GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = GetQuery();

            return cmd;
        }

        public void AssignReferences(IEnumerable<ReferenceXml> references)
        {
            if (!string.IsNullOrEmpty(Reference))
                InitializeFromReferences(references, Reference);
        }

        protected virtual void InitializeFromReferences(IEnumerable<ReferenceXml> references, string value)
        {
            var refChoice = GetReference(references, value);

            if (refChoice.Report == null)
                throw new NullReferenceException(string.Format("A reference named '{0}' has been found, but no element 'report' has been defined", value));

            Initialize(refChoice.Report);
        }

        protected void Initialize(ReportBaseXml reference)
        {
            if (string.IsNullOrEmpty(Source))
                Source = reference.Source;
            
            if (string.IsNullOrEmpty(Path))
                Path = reference.Path;
        }

        protected ReferenceXml GetReference(IEnumerable<ReferenceXml> references, string value)
        {
            if (references == null || references.Count() == 0)
                throw new InvalidOperationException("No reference has been defined for this constraint");

            var refChoice = references.FirstOrDefault(r => r.Name == value);
            if (refChoice == null)
                throw new IndexOutOfRangeException(string.Format("No reference named '{0}' has been defined.", value));
            return refChoice;
        }
    }
}
