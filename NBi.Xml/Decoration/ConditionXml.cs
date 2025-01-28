using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Decoration.Condition;

namespace NBi.Xml.Decoration;

public class ConditionXml
{
    [
        XmlElement(Type = typeof(ServiceRunningConditionXml), ElementName = "service-running"),
        XmlElement(Type = typeof(CustomConditionXml), ElementName = "custom"),
        XmlElement(Type = typeof(FolderExistsConditionXml), ElementName = "folder-exists"),
        XmlElement(Type = typeof(FileExistsConditionXml), ElementName = "file-exists"),
    ]
    public List<DecorationConditionXml> Predicates { get; set; }

    public ConditionXml()
    {
        Predicates = new List<DecorationConditionXml>();
    }
}
