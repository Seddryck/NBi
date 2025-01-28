using System;
using System.Data;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using NBi.Xml.Systems;
using NBi.Xml.Constraints;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Globalization;
using NBi.GenbiL.Templating;
using NBi.Xml;

namespace NBi.GenbiL.Testing.Templating;

[TestFixture]
public class StringTemplateEngineTest
{

    private string ReadTemplateFile(string filename)
    {
        var template = string.Empty;
        // A Stream is needed to read the nbitt document.
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream(
                                       $"{GetType().Namespace}.Resources.{filename}.nbitt"
            ) ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
        {
            template = reader.ReadToEnd();
        }
        return template;
    }

    protected virtual List<List<object>> BuildCase(IEnumerable<string> inlineCase)
    {
        var caseBuilt = new List<List<object>>();
        foreach (var item in inlineCase)
            caseBuilt.Add([item]);

        return caseBuilt;
    }


    [Test]
    public void Build_OrderedLightTemplate_CorrectTest()
    {
        var template = ReadTemplateFile("OrderedLight");
        var variables = new string[] { "perspective", "dimension", "hierarchy", "order" };
        var data = new List<List<List<object>>>()
        {
            BuildCase(["myPerspective", "myDimension", "myHierarchy", "numerical"])
        };
        var engine = new StringTemplateEngine(template, variables);
        var testSuite = engine.Build<TestStandaloneXml>(data, new Dictionary<string, object>());
        var test = testSuite.ElementAt(0);

        //Test the object
        var members = test.Systems[0] as MembersXml;
        Assert.That(members!.Exclude.Items, Is.Null.Or.Empty);
        Assert.That(members!.Exclude.Patterns, Is.Null.Or.Empty);

        var ordered = test.Constraints[0] as OrderedXml;
        Assert.That(ordered!.Rule, Is.EqualTo(OrderedXml.Order.Numerical));
        Assert.That(ordered.Descending, Is.EqualTo(false));
        Assert.That(ordered.Definition, Is.Null.Or.Empty);

        //Test the content serialized
        var content = test.Content;
        Assert.That(content, Does.Contain("rule=\"numerical\""));
        Assert.That(content, Does.Not.Contain("descending=\"false\""));
        Assert.That(content, Does.Not.Contain("<rule-definition"));
        Assert.That(content, Does.Not.Contain("<exclude"));
        
    }

    [Test]
    public void Build_OrderedLightTemplate_ConditionSetupCleanupAreNotAvailable()
    {
        var template = ReadTemplateFile("OrderedLight");
        var variables = new string[] { "perspective", "dimension", "hierarchy", "order" };
        var data = new List<List<List<object>>>()
        {
            BuildCase(["myPerspective", "myDimension", "myHierarchy", "numerical"])
        };
        var engine = new StringTemplateEngine(template, variables);
        var testSuite = engine.Build<TestStandaloneXml>(data, new Dictionary<string, object>());
        var test = testSuite.ElementAt(0);

        //Test the content serialized
        var content = test.Content;
        Assert.That(content, Does.Not.Contain("<setup"));
        Assert.That(content, Does.Not.Contain("<condition"));
        Assert.That(content, Does.Not.Contain("<cleanup"));
    }

    [Test]
    public void Build_OrderedFullTemplate_CorrectTest()
    {
        var template = ReadTemplateFile("OrderedFull");
        var variables = new string[] { "perspective", "dimension", "hierarchy", "order", "descending", "exclude" };
        var data = new List<List<List<object>>>()
        {
                BuildCase(["myPerspective", "myDimension", "myHierarchy", "specific", "true", "Unknown"])
        };

        var engine = new StringTemplateEngine(template, variables);
        var testSuite = engine.Build<TestStandaloneXml>(data, new Dictionary<string, object>());
        var test = testSuite.ElementAt(0);

        //Test the object
        var members = test.Systems[0] as MembersXml;
        Assert.That(members!.Exclude.Items, Is.Not.Null.And.Not.Empty);

        var ordered = test.Constraints[0] as OrderedXml;
        Assert.That(ordered!.Rule, Is.EqualTo(OrderedXml.Order.Specific));
        Assert.That(ordered.Descending, Is.EqualTo(true));
        Assert.That(ordered.Definition, Is.Not.Null.And.Not.Empty);

        //Test the content serialized
        var content = test.Content;
        Assert.That(content, Does.Contain("rule=\"specific\""));
        Assert.That(content, Does.Contain("descending=\"true\""));
        Assert.That(content, Does.Contain("<rule-definition"));
        Assert.That(content, Does.Contain("<exclude"));
    }

    [Test]
    public void BuildTestString_OneSimpleRow_CorrectRendering()
    {
        var template = "<dimension caption='$dimension$' perspective='$perspective$'/>";
        var engine = new StringTemplateEngine(template, ["dimension", "perspective"]);

        var values = new List<List<object>>();
        var firstCell = new List<object>() {"myDim"};
        var secondCell = new List<object>() {"myPersp"};
        values.Add(firstCell);
        values.Add(secondCell);

        engine.InitializeTemplate(new Dictionary<string, object>());
        var result = engine.RenderTemplate(values);

        Assert.That(result, Is.EqualTo("<dimension caption='myDim' perspective='myPersp'/>"));

    }

    [Test]
    public void BuildTestString_OneRowWithMultipleItems_CorrectRendering()
    {
        var template = "$dimension$ ... <subsetOf>\r\n\t<item>$items; separator=\"</item>\r\n\t<item>\"$</item>\r\n</subsetOf>";
        var engine = new StringTemplateEngine(template, ["dimension", "items"]);

        var values = new List<List<object>>();
        var firstCell = new List<object>() { "myDim" };
        var secondCell = new List<object>() { "item A", "item B" };
        values.Add(firstCell);
        values.Add(secondCell);

        engine.InitializeTemplate(new Dictionary<string, object>());
        var result = engine.RenderTemplate(values);

        Assert.That(result, Is.EqualTo("myDim ... <subsetOf>\r\n\t<item>item A</item>\r\n\t<item>item B</item>\r\n</subsetOf>"));
    }

    [Test]
    public void BuildTestString_OneRowWithNoneVariable_CorrectRenderingTextIsIgnored()
    {
        var template = "$dimension$ ... $if(ignore)$<ignore>$ignore$</ignore>$endif$";
        var engine = new StringTemplateEngine(template, ["dimension", "ignore"]);

        var values = new List<List<object>>();
        var firstCell = new List<object>() { "myDim" };
        var secondCell = new List<object>() { "(none)" };
        values.Add(firstCell);
        values.Add(secondCell);

        engine.InitializeTemplate(new Dictionary<string, object>());
        var result = engine.RenderTemplate(values);

        Assert.That(result, Is.EqualTo("myDim ... "));
    }

    [Test]
    public void BuildTestString_OneRowWithEmptyVariable_CorrectRenderingTextIsIgnoredAndVariablePlaceHolderIsEmpty()
    {
        var template = "$dimension$ ->$empty$<- ... $if(empty)$<ignore>$empty$</ignore>$endif$";
        var engine = new StringTemplateEngine(template, ["dimension", "empty"]);

        var values = new List<List<object>>();
        var firstCell = new List<object>() { "myDim" };
        var secondCell = new List<object>() { string.Empty };
        values.Add(firstCell);
        values.Add(secondCell);

        engine.InitializeTemplate(new Dictionary<string, object>());
        var result = engine.RenderTemplate(values);

        Assert.That(result, Is.EqualTo("myDim -><- ... "));
    }

    [Test]
    public void BuildTestString_OneRowWithNotIgnoredVariable_CorrectRenderingTextIsDisplayed()
    {
        var template = "$dimension$ ... $if(ignore)$<ignore>$ignore$</ignore>$endif$";
        var engine = new StringTemplateEngine(template, ["dimension", "ignore"]);

        var values = new List<List<object>>();
        var firstCell = new List<object>() { "myDim" };
        var secondCell = new List<object>() { "reason to ignore" };
        values.Add(firstCell);
        values.Add(secondCell);

        engine.InitializeTemplate(new Dictionary<string, object>());
        var result = engine.RenderTemplate(values);

        Assert.That(result, Is.EqualTo("myDim ... <ignore>reason to ignore</ignore>"));
    }

    [Test]
    public void BuildTestString_EncodeXml_CorrectEncoding()
    {
        var template = "<element attribute=\"$value; format=\"xml-encode\"$\" other-attribute=\"$other$\">";
        var engine = new StringTemplateEngine(template, ["value", "other"]);

        var values = new List<List<object>>();
        var firstCell = new List<object>() { "<value&"};
        var secondCell = new List<object>() { "<value&" };
        values.Add(firstCell);
        values.Add(secondCell);

        engine.InitializeTemplate(new Dictionary<string, object>());
        var result = engine.RenderTemplate(values);

        Assert.That(result, Is.EqualTo("<element attribute=\"&lt;value&amp;\" other-attribute=\"<value&\">"));
    }
}
