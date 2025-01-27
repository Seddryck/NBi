using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class RoutineParameterXml : RoutineXml, IPerspectiveFilter, IRoutineFilter, IResultFilter, IParameterDirectionFilter
{

    [XmlAttribute("routine")]
    public string Routine { get; set; }

    [XmlAttribute("result")]
    [DefaultValue(IsResultOption.Unspecified)]
    public IsResultOption IsResult { get; set; }

    [XmlAttribute("direction")]
    [DefaultValue(ParameterDirectionOption.Unspecified)]
    public ParameterDirectionOption Direction { get; set; }

    [XmlIgnore]
    protected override string Path { get { return string.Format("[{0}].[{1}]", Routine, Caption); } }

    public override string TypeName
    {
        get { return "routine's parameter"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:routine", Routine);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(Routine))
            values.Add(string.Format("Routine '{0}'", Routine));
        values.Add(string.Format("Parameter '{0}'", Caption));
        values.Add("Parameters");
        return values;
    }

    
}
