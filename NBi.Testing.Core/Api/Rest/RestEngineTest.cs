using NBi.Core.Api.Authentication;
using NBi.Core.Api.Rest;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Api.Rest;

public class RestEngineTest
{
    [Test]
    public void Execute_OneParameter_CorrectResponse()
    {
        var baseUrl = new LiteralScalarResolver<string>("https://api.agify.io/");
        var parameter = new ParameterRest(
            new LiteralScalarResolver<string>("name"),
            new LiteralScalarResolver<string>("cedric")
        );
        var engine = new RestEngine(new Anonymous(), baseUrl, new LiteralScalarResolver<string>(""), [parameter], [], []);
        var result = engine.Execute();
        Assert.That(result, Does.Contain("\"name\":\"cedric\",\"age\":"));
    }

    [Test]
    [Ignore("Public API has changed")]
    public void Execute_PathAndParameters_CorrectResponse()
    {
        var baseUrl = new LiteralScalarResolver<string>("https://api.publicapis.org/");
        var path = new LiteralScalarResolver<string>("entries");
        var parameter1 = new ParameterRest(
            new LiteralScalarResolver<string>("title"),
            new LiteralScalarResolver<string>("animal")
        );
        var parameter2 = new ParameterRest(
            new LiteralScalarResolver<string>("https"),
            new LiteralScalarResolver<string>("true")
        );
        var engine = new RestEngine(new Anonymous(), baseUrl, path, [parameter1, parameter2], [], []);
        var result = engine.Execute();
        Assert.That(result.Length, Is.GreaterThan(20));
        Assert.That(result, Does.StartWith("{\"count\":"));
    }


    //[Test]
    //public void Execute_Segments_CorrectResponse()
    //{
    //    var baseUrl = new LiteralScalarResolver<string>("https://verse.pawelad.xyz/");
    //    var path = new LiteralScalarResolver<string>("/projects/{project}/");
    //    var segment = new SegmentRest(
    //        new LiteralScalarResolver<string>("project"),
    //        new LiteralScalarResolver<string>("jekyll")
    //    );
    //    var parameter = new ParameterRest(
    //        new LiteralScalarResolver<string>("format"),
    //        new LiteralScalarResolver<string>("json")
    //    );
    //    var engine = new RestEngine(new Anonymous(), baseUrl, path, new[] { parameter }, new[] { segment }, null);
    //    var result = engine.Execute();
    //    Assert.That(result, Does.StartWith("{\"latest\":"));
    //}

    [Test]
    [Ignore("We need to mock HTTP response because API are changing too often")]
    public void Execute_Segments_CorrectResponse()
    {
        var baseUrl = new LiteralScalarResolver<string>("http://api.icndb.com");
        var path = new LiteralScalarResolver<string>("/jokes/{id}");
        var segment = new SegmentRest(
            new LiteralScalarResolver<string>("id"),
            new LiteralScalarResolver<string>("268")
        );
        var parameter1 = new ParameterRest(
            new LiteralScalarResolver<string>("firstName"),
            new LiteralScalarResolver<string>("John")
        );
        var parameter2 = new ParameterRest(
            new LiteralScalarResolver<string>("firstName"),
            new LiteralScalarResolver<string>("John")
        );
        var engine = new RestEngine(new Anonymous(), baseUrl, path, [parameter1, parameter2], [segment], []);
        var result = engine.Execute();
        Assert.That(result, Does.StartWith("{ \"type\": \"success\", \"value\": { \"id\": 268,"));
    }
}
