using System.Collections.Generic;
using NUnit.Framework;
using NBi.Core.Structure;
using NBi.Core.Structure.Olap.Builders;

namespace NBi.Core.Testing.Structure.Olap.Builders;

[TestFixture]
public class HierarchyDiscoveryCommandBuilderTest
{
    [Test]
    public void GetCommandText_FiltersContainDisplayFolder_DisplayFolderIsNotInCommandFilter()
    {
        var filters  = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "my perspective")
            , new CaptionFilter(Target.Dimensions, "my dimension")
            , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            , new CaptionFilter(Target.Hierarchies, "my hierarchy")
        };

        var builder = new HierarchyDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();

        Assert.That(commandText, Does.Not.Contain("Display").And.Not.Contain("Folder"));

    }

    [Test]
    public void GetCommandText_FiltersContainDisplayFolder_CommandFiltersDoesNotContainDoubleAnd()
    {
        var filters  = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "my perspective")
            , new CaptionFilter(Target.Dimensions, "my dimension")
            , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            , new CaptionFilter(Target.Hierarchies, "my hierarchy")
        };

        var builder = new HierarchyDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();

        Assert.That(commandText.Replace(" ","").ToLower(), Does.Not.Contain("andand"));
    }

    [Test]
    public void Build_FiltersContainDisplayFolder_PostCommandFilterIsNotEmpty()
    {
        var filters  = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "my perspective")
            , new CaptionFilter(Target.Dimensions, "my dimension")
            , new CaptionFilter(Target.DisplayFolders, "my display-folder")
            , new CaptionFilter(Target.Hierarchies, "my hierarchy")
        };

        var builder = new HierarchyDiscoveryCommandBuilder();
        builder.Build(filters);
        var postFilters = builder.GetPostFilters();

        Assert.That(postFilters, Is.Not.Null.And.Not.Empty);
    }

}
