using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Hierarchy : Dimension, IDimensionFilter, IDisplayFolderFilter
{
    public Hierarchy()
    {
        Specification = new SpecificationHierarchy();
    }

    public string Dimension { get; set; }

    protected string displayFolder;
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

    public SpecificationHierarchy Specification { get; protected set; }

    public class SpecificationHierarchy
    {
        public bool IsDisplayFolderSpecified { get; internal set; }
    }

    protected virtual string ParentPath { get { return string.Format("[{0}]", Dimension); } }
    protected  override string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }

    public override string TypeName
    {
        get { return "hierarchy"; }
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
        values.Add(string.Format("Hierarchy '{0}'", Caption));
        values.Add("Hierarchies");
        return values;
    }
}
