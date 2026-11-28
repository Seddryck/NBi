using NBi.Model.Decoration;
using NBi.Model.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Model;

public abstract class InheritanceTest
{
    protected List<string> categories;
    protected List<Trait> traits;

    protected Setup setup;

    protected Cleanup cleanup;

    public InheritanceTest()
    {
        categories = new List<string>();
        traits = new List<Trait>();
        setup = new Setup();
        cleanup = new Cleanup();
    }

    public void AddInheritance(List<string> inheritedCategories, Setup inheritedSetup, Cleanup inheritedCleanup)
    {
        this.categories.AddRange(inheritedCategories);
        InheritDecoration(this.setup, inheritedSetup);
        InheritDecoration(this.cleanup, inheritedCleanup);
    }

    protected void InheritDecoration(DecorationDefinition obj, DecorationDefinition decoration)
    {
        if (decoration != null && decoration.Commands.Count > 0)
        {
            if (obj == null || decoration.Commands.Count == 0)
            {
                obj = decoration;
            }
            else
            {
                for (int i = 0; i < decoration.Commands.Count; i++)
                    obj.Commands.Insert(i, decoration.Commands[i]);
            }
        }
    }
}
