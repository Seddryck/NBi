using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Query.Client;
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
        public virtual string InlineQuery
        {
            get { return inlineQuery; }
            set { inlineQuery = value; }
        }

        [XmlElement("assembly")]
        public AssemblyXml Assembly { get; set; }

        [XmlElement("report")]
        public virtual ReportXml Report { get; set; }

        [XmlElement("shared-dataset")]
        public virtual SharedDatasetXml SharedDataset { get; set; }

    }
}