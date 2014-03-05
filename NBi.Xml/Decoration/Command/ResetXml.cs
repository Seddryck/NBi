using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.DataManipulation;

namespace NBi.Xml.Decoration.Command
{
    public class ResetXml : DataManipulationAbstractXml, IResetCommand
    {
        [XmlAttribute("table")]
        public string TableName {get; set;}
        
    }
}
