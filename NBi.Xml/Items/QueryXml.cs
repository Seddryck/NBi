using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.SerializationOption;

namespace NBi.Xml.Items
{
    public class QueryXml : QueryableXml
    {
        
        [XmlAttribute("file")]
        public string File { get; set; }
        
        [XmlAttribute("connectionString-ref")]
        public string ConnectionStringReference { get; set; }

        [XmlIgnore]
        private string inlineQuery;

        [XmlIgnore]
        public CData InlineQueryWrite
        {
            get { return inlineQuery; }
            set { inlineQuery = value; }
        }

        [XmlText]
        public string InlineQuery
        {
            get { return inlineQuery; }
            set { inlineQuery = value; }
        }

        public override string GetQuery()
        {
            //if Sql is specified then return it
            if (InlineQuery!= null && !string.IsNullOrEmpty(InlineQuery))
                return InlineQuery;

            if (string.IsNullOrEmpty(File))
                throw new InvalidOperationException("Element query must contain a query or a file!");

            //Else check that file exists and read the file's content
            var file = string.Empty;
            if (Path.IsPathRooted(File))
                file = File;
            else
                file = Settings.BasePath + File;
            if (!System.IO.File.Exists(file))
                throw new ExternalDependencyNotFoundException(file);
            var query = System.IO.File.ReadAllText(file, Encoding.UTF8);
            return query;
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