using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Report;

namespace NBi.Xml.Items
{
    public class ReportXml : QueryableXml
    {
        [XmlAttribute("source")]
        public string Source { get; set; }
        
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("dataset")]
        public string Dataset { get; set; }

        [XmlElement("parameter")]
        public List<QueryParameterXml> Parameters { get; set; }

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


        public List<QueryParameterXml> GetParameters()
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
    }
}
