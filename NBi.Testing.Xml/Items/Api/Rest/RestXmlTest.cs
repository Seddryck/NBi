using NBi.Xml.Items;
using NBi.Xml.Items.Api.Authentication;
using NBi.Xml.Items.Api.Rest;
using NBi.Xml.Items.Hierarchical.Json;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Testing.Items.Api.Rest;

[TestFixture]
public class RestXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_TestUsingRest_RestNotNull()
    {
        int testNr = 0;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var resultSet = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];
        Assert.That(resultSet.JsonSource, Is.Not.Null);
        var jsonSource = resultSet.JsonSource!;
        Assert.That(jsonSource.Rest, Is.Not.Null);
    }

    [Test]
    public void Deserialize_TestUsingRestWithParameters_RestNotNull()
    {
        int testNr = 0;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var rest = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).JsonSource!.Rest;
        Assert.That(rest.Parameters, Is.Not.Null);
        Assert.That(rest.Parameters, Has.Count.EqualTo(2));
        Assert.That(rest.Parameters.Any(x => x.Name == "parameter1"));
        Assert.That(rest.Parameters.Any(x => x.Value == "value1"));
    }

    [Test]
    public void Deserialize_TestUsingRestWithHeaders_RestNotNull()
    {
        int testNr = 0;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var rest = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).JsonSource!.Rest;
        Assert.That(rest.Headers, Is.Not.Null);
        Assert.That(rest.Headers, Has.Count.EqualTo(2));
        Assert.That(rest.Headers.Any(x => x.Name == "header1"));
        Assert.That(rest.Headers.Any(x => x.Value == "value1"));
    }

    [Test]
    public void Deserialize_TestUsingRestWithSegment_RestNotNull()
    {
        int testNr = 0;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var rest = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).JsonSource!.Rest;
        Assert.That(rest.Segments, Is.Not.Null);
        Assert.That(rest.Segments, Has.Count.EqualTo(2));
        Assert.That(rest.Segments.Any(x => x.Name == "segment1"));
        Assert.That(rest.Segments.Any(x => x.Value == "value1"));
    }

    [Test]
    public void Deserialize_TestUsingRestWithoutAuthentication_AnonymousSelected()
    {
        int testNr = 0;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var rest = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).JsonSource!.Rest;
        Assert.That(rest.Authentication.Protocol, Is.TypeOf<AnonymousXml>());
    }

    [Test]
    public void Deserialize_TestUsingRestWithAnonyous_AnonymousValid()
    {
        int testNr = 1;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var rest = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).JsonSource!.Rest;
        Assert.That(rest.Authentication.Protocol, Is.TypeOf<AnonymousXml>());
    }

    [Test]
    public void Deserialize_TestUsingRestWithApiKey_ApiKeyValid()
    {
        int testNr = 2;
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var rest = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).JsonSource!.Rest;
        Assert.That(rest.Authentication.Protocol, Is.TypeOf<ApiKeyXml>());
        var authentication = (ApiKeyXml)rest.Authentication.Protocol;
        Assert.That(authentication.Name, Is.EqualTo("apiKey"));
        Assert.That(authentication.Value, Is.EqualTo("123456"));
    }

    [Test]
    public void Serialize_TestUsingRestWithAnonymous_AnonymousNotAdded()
    {
        var jsonSource = new JsonSourceXml
        {
            Rest = new RestXml
            {
                Authentication = new AuthenticationXml { Protocol = new AnonymousXml() },
                BaseAddress = "https://api.website.com",
                Headers = [new RestHeaderXml { Name = "rest-header-1", Value = "rh-val1" }],
                Path = new RestPathXml { Value = "v2/{user}/tags/{tag}" },
                Segments = [new RestSegmentXml { Name = "user", Value = "xyz" }, new RestSegmentXml { Name = "tag", Value = "up" }],
            }
        };

        var serializer = new XmlSerializer(jsonSource.GetType());
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, jsonSource);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<rest base-address="));
            Assert.That(content, Does.Contain("https://api.website.com"));
            Assert.That(content, Does.Contain("<header name=\"rest-header-1\""));
            Assert.That(content, Does.Contain("<path>"));
            Assert.That(content, Does.Contain("v2/{user}/tags/{tag}"));
            Assert.That(content, Does.Contain("<segment name=\"user\""));
            Assert.That(content, Does.Contain("<segment name=\"tag\""));

            Assert.That(content, Does.Not.Contain("<authentication"));
        }
    }

    [Test]
    public void Serialize_TestUsingRestWithApiKey_ApiKeyAdded()
    {
        var jsonSource = new JsonSourceXml
        {
            Rest = new RestXml
            {
                Authentication = new AuthenticationXml { Protocol = new ApiKeyXml { Value = "123456" } },
                BaseAddress = "https://api.website.com",
            }
        };

        var serializer = new XmlSerializer(jsonSource.GetType());
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, jsonSource);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<rest base-address="));
            Assert.That(content, Does.Contain("https://api.website.com"));
            Assert.That(content, Does.Not.Contain("<header"));
            Assert.That(content, Does.Not.Contain("<segment"));
            Assert.That(content, Does.Not.Contain("<parameter"));
            Assert.That(content, Does.Not.Contain("<path>"));

            Assert.That(content, Does.Contain("<authentication"));
            Assert.That(content, Does.Contain("<api-key"));
            Assert.That(content, Does.Contain("123456"));
        }
    }
}
