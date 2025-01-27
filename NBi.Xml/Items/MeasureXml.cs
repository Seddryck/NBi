using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class MeasureXml : MeasureGroupXml, IMeasureGroupFilter, IDisplayFolderFilter
{
    protected string measureGroup;
    [XmlAttribute("measure-group")]
    public string MeasureGroup
    {
        get
        { return measureGroup; }

        set
        {
            measureGroup = value;
            Specification.IsMeasureGroupSpecified = true;
        }
    }

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

    public MeasureXml()
    {
        Specification = new SpecificationMeasure();
    }

    [XmlIgnore()]
    public SpecificationMeasure Specification { get; protected set; }

    public class SpecificationMeasure
    {
        public bool IsDisplayFolderSpecified { get; internal set; }
        public bool IsMeasureGroupSpecified { get; internal set; }
    }

    [XmlIgnore]
    public override string TypeName
    {
        get { return "measure"; }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        dico.Add("sut:measure-group", MeasureGroup);
        dico.Add("sut:display-folder", DisplayFolder);
        return dico;
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        if (!string.IsNullOrEmpty(Perspective))
            values.Add(string.Format("Perspective '{0}'", Perspective));
        if (!string.IsNullOrEmpty(MeasureGroup))
            values.Add(string.Format("Measure group '{0}'", MeasureGroup));
        values.Add("Measures");
        return values;
    }
}
