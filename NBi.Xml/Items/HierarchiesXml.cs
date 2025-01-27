using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class HierarchiesXml : DimensionsXml, IDimensionFilter, IDisplayFolderFilter
{
    public HierarchiesXml()
    {
        Specification = new SpecificationHierarchy();
    }
    
    [XmlAttribute("dimension")]
    public string Dimension { get; set; }

    protected string displayFolder;
    [XmlAttribute("display-folder")]
    public string DisplayFolder
    {
        get
        { return displayFolder; }

        set
        {
            displayFolder = value;
            Specification.IsDisplayFolderSpecified = true;
        }
    }

    [XmlIgnore()]
    public SpecificationHierarchy Specification { get; protected set; }

    public class SpecificationHierarchy
    {
        public bool IsDisplayFolderSpecified { get; internal set; }
    }

    [XmlIgnore]
    protected virtual string ParentPath { get { return string.Format("[{0}]", Dimension); } }
    [XmlIgnore]
    protected  override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

    [XmlIgnore]
    public override string TypeName
    {
        get { return "hierarchies"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:dimension", Dimension);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(Dimension))
            values.Add(string.Format("Dimension '{0}'", Dimension));
        values.Add("Hierarchies");
        return values;
    }
}
