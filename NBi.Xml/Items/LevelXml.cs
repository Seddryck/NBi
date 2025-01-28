using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class LevelXml : DimensionXml, IHierarchyFilter, IDimensionFilter
{
    [XmlAttribute("dimension")]
    public string Dimension { get; set; }

    [XmlAttribute("hierarchy")]
    public string Hierarchy { get; set; }

    [XmlIgnore]
    protected virtual string ParentPath { get { return string.Format("[{0}].[{1}]", Dimension, Hierarchy); } }

    [XmlIgnore]
    protected override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

    [XmlIgnore]
    public override string TypeName
    {
        get { return "level"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:dimension", Dimension);
        dico.Add("sut:hierarchy", Hierarchy);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(Dimension))
            values.Add(string.Format("Dimension '{0}'", Dimension));
        if (!string.IsNullOrEmpty(Hierarchy))
            values.Add(string.Format("Hierarchy '{0}'", Hierarchy));
        values.Add(string.Format("Level '{0}'", Caption));
        values.Add("Levels");
        return values;
    }
}
