using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.DataManipulation;

namespace NBi.Xml.Decoration.Command
{
    public class TableResetXml : DataManipulationAbstractXml, IResetCommand
    {
        [XmlAttribute("name")]
        public string TableName {get; set;}
        
    }
}
