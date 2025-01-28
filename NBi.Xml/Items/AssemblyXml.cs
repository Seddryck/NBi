using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Assemblies;

namespace NBi.Xml.Items;

public class AssemblyXml : QueryableXml
{
    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("class")]
    public string Klass { get; set; }

    [XmlAttribute("method")]
    public string Method { get; set; }

    [XmlAttribute("static")]
    public bool Static { get; set; }

    [XmlElement("method-parameter")]
    public List<AssemblyParameterXml> MethodParameters { get; set; }

    public AssemblyXml()
    {
        MethodParameters = new List<AssemblyParameterXml>();
    }


    public Dictionary<string, object> GetMethodParameters()
    {
        var dico = new Dictionary<string, object>();

        foreach (var param in this.MethodParameters)
        {
            dico.Add(param.Name, param.Value);
        }
        return dico;
    }

}
