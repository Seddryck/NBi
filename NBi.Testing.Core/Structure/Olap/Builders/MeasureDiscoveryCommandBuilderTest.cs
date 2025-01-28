using NBi.Core.Structure;
using NBi.Core.Structure.Olap.Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Structure.Olap.Builders;

public class MeasureDiscoveryCommandBuilderTest
{
    [Test]
    public void GetCommandText_CubeFilter_CorrectStatement()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
        };

        var builder = new MeasureDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();
        Assert.That(commandText.Replace(" ","").ToLower(), Does.Contain("[cube_name]='cubeName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Not.Contain("[measuregroup_name]=".ToLower()));
    }

    [Test]
    public void GetCommandText_CubeFilterAndMeasureGroupFilter_CorrectStatement()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
            , new CaptionFilter(Target.MeasureGroups, "measureGroupName")
        };

        var builder = new MeasureDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[cube_name]='cubeName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[measuregroup_name]='measureGroupName'".ToLower()));
    }

    [Test]
    public void GetCommandText_CubeFilterMeasureGroupAndMeasureFilter_CorrectStatement()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
            , new CaptionFilter(Target.MeasureGroups, "measureGroupName")
            , new CaptionFilter(Target.Measures, "measureName")
        };

        var builder = new MeasureDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[cube_name]='cubeName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[measuregroup_name]='measureGroupName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[measure_caption]='measureName'".ToLower()));
    }

    [Test]
    public void GetCommandText_CubeFilterMeasureGroupAndMeasureAndDisplayFolderFilter_CorrectStatement()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
            , new CaptionFilter(Target.MeasureGroups, "measureGroupName")
            , new CaptionFilter(Target.Measures, "measureName")
            , new CaptionFilter(Target.DisplayFolders, "displayFolderName")
        };

        var builder = new MeasureDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[cube_name]='cubeName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[measuregroup_name]='measureGroupName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[measure_caption]='measureName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Not.Contain("displayFolderName".ToLower()));
    }

    [Test]
    public void GetCommandText_CubeFilterMeasureGroupAndMeasureAndDisplayFolderFilter_CorrectPostFilter()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
            , new CaptionFilter(Target.MeasureGroups, "measureGroupName")
            , new CaptionFilter(Target.Measures, "measureName")
            , new CaptionFilter(Target.DisplayFolders, "displayFolderName")
        };

        var builder = new MeasureDiscoveryCommandBuilder();
        builder.Build(filters);
        var postFilters = builder.GetPostFilters();
        Assert.That(postFilters.Count(), Is.EqualTo(1));
    }
}
