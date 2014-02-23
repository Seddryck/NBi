using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.DataManipulation;

namespace NBi.Xml.Decoration.Command
{
    public abstract class DecorationCommandXml : IDataManipulationCommand
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }
    }
}
