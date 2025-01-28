using NBi.Xml.Decoration;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml;

public abstract class InheritanceTestXml
{
    protected List<string> categories;
    protected List<TraitXml> traits;

    protected SetupXml setup;

    protected CleanupXml cleanup;

    public InheritanceTestXml()
    {
        categories = new List<string>();
        traits = new List<TraitXml>();
        setup = new SetupXml();
        cleanup = new CleanupXml();
    }

    public void AddInheritance(List<string> inheritedCategories, SetupXml inheritedSetup, CleanupXml inheritedCleanup)
    {
        this.categories.AddRange(inheritedCategories);
        InheritDecoration(this.setup, inheritedSetup);
        InheritDecoration(this.cleanup, inheritedCleanup);
    }

    protected void InheritDecoration(DecorationXml obj, DecorationXml decoration)
    {
        if (decoration != null && decoration.Commands.Count > 0)
            if (obj == null || decoration.Commands.Count == 0)
                obj = decoration;
            else
            {
                for (int i = 0; i < decoration.Commands.Count; i++)
                    obj.Commands.Insert(i, decoration.Commands[i]);
            }
    }
}
