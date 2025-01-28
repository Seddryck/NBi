using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using NBi.Xml.Items.Format;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class MatchPatternXmlTest
{
    protected TestSuiteXml DeserializeSample(string descriminator)
    {
        // Declare an object variable of the type to be deserialized.
        var manager = new XmlManager();

        // A Stream is needed to read the XML document.
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream(
                                       $"{GetType().Assembly.GetName().Name}.Resources.MatchPatternXml{descriminator}TestSuite.xml")
                                       ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
        {
            manager.Read(reader);
        }
        manager.ApplyDefaultSettings();
        return manager.TestSuite!;
    }
    
    [Test]
    public void Deserialize_SampleFile_MatchPatternRegex()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("");

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());
        var matchPatternConstraint = (MatchPatternXml)ts.Tests[testNr].Constraints[0];
        
        Assert.That(matchPatternConstraint.Regex, Is.EqualTo(@"^[2-9]\d{2}-\d{3}-\d{4}$"));
    }

    [Test]
    public void Deserialize_SampleFile_MatchPatternNumericFormat()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("WithReference");

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());
        var matchPatternConstraint = (MatchPatternXml)ts.Tests[testNr].Constraints[0];

        Assert.That(matchPatternConstraint.NumericFormat, Is.Not.Null);
        Assert.That(matchPatternConstraint.NumericFormat.DecimalDigits, Is.EqualTo(3));
        Assert.That(matchPatternConstraint.NumericFormat.DecimalSeparator, Is.EqualTo(","));
        Assert.That(matchPatternConstraint.NumericFormat.GroupSeparator, Is.EqualTo(" "));
    }

    [Test]
    public void Deserialize_SampleFile_MatchPatternNumericFormatWithoutReference()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("WithReference");

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<MatchPatternXml>());
        var matchPatternConstraint = (MatchPatternXml)ts.Tests[testNr].Constraints[0];

        Assert.That(matchPatternConstraint.NumericFormat, Is.Not.Null);
        Assert.That(matchPatternConstraint.NumericFormat.DecimalDigits, Is.EqualTo(2));
        Assert.That(matchPatternConstraint.NumericFormat.DecimalSeparator, Is.EqualTo("."));
        Assert.That(matchPatternConstraint.NumericFormat.GroupSeparator, Is.EqualTo(""));
    }

    [Test]
    public void Serialize_MatchPatternWithRegex_RegexButNoOtherElement()
    {
        var matchPattern = new MatchPatternXml
        {
            Regex = "^regex+"
        };

        var manager = new XmlManager();
        var str = manager.XmlSerializeFrom(matchPattern);

        var xml = new System.Xml.XmlDocument();
        xml.LoadXml(str);
        var node = xml.ChildNodes[1];

        Assert.That(node!.ChildNodes[0]!.Name, Is.EqualTo("regex"));
        Assert.That(node.ChildNodes[0]!.InnerText, Is.EqualTo("^regex+"));
        Assert.That(node.ChildNodes, Has.Count.EqualTo(1));
    }

    [Test]
    public void Serialize_MatchPatternWithNumericFormat_NumericFormatButNoOtherElement()
    {
        var matchPattern = new MatchPatternXml();
        matchPattern.NumericFormat.DecimalDigits = 2;


        var manager = new XmlManager();
        var str = manager.XmlSerializeFrom(matchPattern);

        var xml = new System.Xml.XmlDocument();
        xml.LoadXml(str);
        var node = xml.ChildNodes[1]!;

        Assert.That(node.ChildNodes[0]!.Name, Is.EqualTo("numeric-format"));
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"]!.Value, Is.EqualTo("2"));
        Assert.That(node.ChildNodes!, Has.Count.EqualTo(1));
        Assert.That(node.ChildNodes[0]!.Attributes, Has.Count.EqualTo(1));
    }

    [Test]
    public void Serialize_MatchPatternWithNumericFormatAndGroupSeparator_NumericFormatButNoOtherElement()
    {
        var matchPattern = new MatchPatternXml();
        matchPattern.NumericFormat.DecimalDigits = 5;
        matchPattern.NumericFormat.GroupSeparator = " ";


        var manager = new XmlManager();
        var str = manager.XmlSerializeFrom(matchPattern);

        var xml = new System.Xml.XmlDocument();
        xml.LoadXml(str);
        var node = xml.ChildNodes[1]!;

        Assert.That(node.ChildNodes[0]!.Name, Is.EqualTo("numeric-format"));
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"]!.Value, Is.EqualTo("5"));
        Assert.That(node.ChildNodes[0]!.Attributes!["group-separator"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["group-separator"]!.Value, Is.EqualTo(" "));
        Assert.That(node.ChildNodes, Has.Count.EqualTo(1));
        Assert.That(node.ChildNodes[0]!.Attributes, Has.Count.EqualTo(2));
    }

    [Test]
    public void Serialize_MatchPatternWithNumericFormatAndFull_NumericFormatButNoOtherElement()
    {
        var matchPattern = new MatchPatternXml();
        matchPattern.NumericFormat.DecimalDigits = 5;
        matchPattern.NumericFormat.GroupSeparator = " ";
        matchPattern.NumericFormat.DecimalSeparator = ",";


        var manager = new XmlManager();
        var str = manager.XmlSerializeFrom(matchPattern);

        var xml = new System.Xml.XmlDocument();
        xml.LoadXml(str);
        var node = xml.ChildNodes[1]!;

        Assert.That(node.ChildNodes[0]!.Name, Is.EqualTo("numeric-format"));
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"]!.Value, Is.EqualTo("5"));
        Assert.That(node.ChildNodes[0]!.Attributes!["group-separator"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["group-separator"]!.Value, Is.EqualTo(" "));
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-separator"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-separator"]!.Value, Is.EqualTo(","));
        Assert.That(node.ChildNodes, Has.Count.EqualTo(1));
        Assert.That(node.ChildNodes[0]!.Attributes, Has.Count.EqualTo(3));
    }

    [Test]
    public void Serialize_MatchPatternWithCurrencyFormat_CurrencyFormatButNoOtherElement()
    {
        var matchPattern = new MatchPatternXml();
        matchPattern.CurrencyFormat.DecimalDigits = 2;
        matchPattern.CurrencyFormat.CurrencyPattern = NBi.Core.Format.CurrencyPattern.PrefixSpace;
        matchPattern.CurrencyFormat.CurrencySymbol = "£";


        var manager = new XmlManager();
        var str = manager.XmlSerializeFrom(matchPattern);

        var xml = new System.Xml.XmlDocument();
        xml.LoadXml(str);
        var node = xml.ChildNodes[1]!;

        Assert.That(node.ChildNodes[0]!.Name, Is.EqualTo("currency-format"));
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"]!.Value, Is.EqualTo("2"));
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-pattern"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-pattern"]!.Value, Is.EqualTo("$ n"));
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-symbol"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-symbol"]!.Value, Is.EqualTo("£"));
        Assert.That(node.ChildNodes, Has.Count.EqualTo(1));
        Assert.That(node.ChildNodes[0]!.Attributes, Has.Count.EqualTo(3));
    }

    [Test]
    public void Serialize_MatchPatternWithCurrencyFormatLight_CurrencyFormatButNoOtherElement()
    {
        var matchPattern = new MatchPatternXml();
        matchPattern.CurrencyFormat.CurrencyPattern = NBi.Core.Format.CurrencyPattern.PrefixSpace;
        matchPattern.CurrencyFormat.CurrencySymbol = "£";


        var manager = new XmlManager();
        var str = manager.XmlSerializeFrom(matchPattern);

        var xml = new System.Xml.XmlDocument();
        xml.LoadXml(str);
        var node = xml.ChildNodes[1]!;

        Assert.That(node.ChildNodes[0]!.Name, Is.EqualTo("currency-format"));
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-pattern"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-pattern"]!.Value, Is.EqualTo("$ n"));
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-symbol"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["currency-symbol"]!.Value, Is.EqualTo("£"));
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"], Is.Not.Null);
        Assert.That(node.ChildNodes[0]!.Attributes!["decimal-digits"]!.Value, Is.EqualTo("0"));
        Assert.That(node.ChildNodes, Has.Count.EqualTo(1));
        Assert.That(node.ChildNodes[0]!.Attributes, Has.Count.EqualTo(3));
    }
}
