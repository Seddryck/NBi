using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Decoration.Command;

namespace NBi.Xml.Decoration
{
    public abstract class DecorationXml
    {
        [
        XmlElement(Type = typeof(SqlRunXml), ElementName = "sql-run"),
        XmlElement(Type = typeof(TableLoadXml), ElementName = "table-load"),
        XmlElement(Type = typeof(TableResetXml), ElementName = "table-reset"),
        XmlElement(Type = typeof(ServiceStartXml), ElementName = "service-start"),
        XmlElement(Type = typeof(ServiceStopXml), ElementName = "service-stop"),
        XmlElement(Type = typeof(ExeRunXml), ElementName = "exe-run"),
        XmlElement(Type = typeof(FileDeleteXml), ElementName = "file-delete"),
        XmlElement(Type = typeof(FileCopyXml), ElementName = "file-copy"),
        XmlElement(Type = typeof(EtlRunXml), ElementName = "etl-run"),
        XmlElement(Type = typeof(CommandGroupXml), ElementName = "tasks")
        ]
        public List<DecorationCommandXml> Commands { get; set; }


        public DecorationXml()
        {
            Commands = new List<DecorationCommandXml>();
        }
    }
}
