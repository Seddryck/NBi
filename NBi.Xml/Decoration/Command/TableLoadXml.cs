using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;
using NBi.Extensibility;

namespace NBi.Xml.Decoration.Command;

public class TableLoadXml : DataManipulationAbstractXml
{
    [XmlAttribute("name")]
    public string TableName { get; set; }
    
    [XmlAttribute("file")]
    public string InternalFileName { get; set; }

    [XmlIgnore]
    public string FileName
    {
        get
        {
            var file = string.Empty;
            if (Path.IsPathRooted(InternalFileName))
                file = InternalFileName;
            else
                file = (Settings?.BasePath ?? string.Empty) + InternalFileName;

            return file;
        }
    }
}
