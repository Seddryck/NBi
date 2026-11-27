using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Model.Xml.Decoration.Command;

public class TableResetXml : DataManipulationAbstractXml
{
    [XmlAttribute("name")]
    public string TableName {get; set;}

}
