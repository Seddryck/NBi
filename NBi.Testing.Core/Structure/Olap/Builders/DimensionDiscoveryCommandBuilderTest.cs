using NBi.Core.Structure;
using NBi.Core.Structure.Olap.Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Structure.Olap.Builders;

public class DimensionDiscoveryCommandBuilderTest
{
    [Test]
    public void GetCommandText_CubeFilter_CorrectStatement()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
        };

        var builder = new DimensionDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();
        Assert.That(commandText.Replace(" ","").ToLower(), Does.Contain("[cube_name]='cubeName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Not.Contain("[dimension_caption]=".ToLower()));
    }

    [Test]
    public void GetCommandText_CubeFilterAndDimensionFilter_CorrectStatement()
    {
        var filters = new CaptionFilter[]
        {
            new CaptionFilter(Target.Perspectives, "cubeName")
            , new CaptionFilter(Target.Dimensions, "dimensionName")
        };

        var builder = new DimensionDiscoveryCommandBuilder();
        builder.Build(filters);
        var commandText = builder.GetCommandText();
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[cube_name]='cubeName'".ToLower()));
        Assert.That(commandText.Replace(" ", "").ToLower(), Does.Contain("[dimension_caption]='dimensionName'".ToLower()));
    }
}
