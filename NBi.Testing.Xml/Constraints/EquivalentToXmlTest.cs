#region Using directives
using System;
using System.IO;
using System.Reflection;
using NBi.Core.Members.Predefined;
using NBi.Core.Members.Ranges;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items.Ranges;
using NUnit.Framework;
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class EquivalentToXmlTest : BaseXmlTest
{

    [Test]
    public void DeserializeEquivalentToItems_ListOfItems_Inline()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EquivalentToXml>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items, Is.Not.Null);
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("Hello"));
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("World"));
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).GetItems(), Has.Count.EqualTo(2));
    }

    [Test]
    public void DeserializeEquivalentToOneColumnQuery_SqlQuery_Inline()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EquivalentToXml>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Query, Is.Not.Null);
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Query!.InlineQuery, Does.Contain("Hello").And.Contain("World"));
    }

    [Test]
    public void DeserializeEquivalentTo_PredefinedItems_DaysOfWeek()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EquivalentToXml>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).PredefinedItems, Is.Not.Null);
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).PredefinedItems!.Type, Is.EqualTo(PredefinedMembers.DaysOfWeek));
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).PredefinedItems!.Language, Is.EqualTo("en"));
    }

    [Test]
    public void DeserializeEquivalentTo_IntegerRange_1To10Step2()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EquivalentToXml>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.Not.Null);
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.InstanceOf<IIntegerRange>());

        var range = (IIntegerRange)((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range!;
        Assert.That(range.Start, Is.EqualTo(1));
        Assert.That(range.End, Is.EqualTo(10));
        Assert.That(range.Step, Is.EqualTo(2));
    }

    [Test]
    public void DeserializeEquivalentTo_DateRange_1JanuaryTo31December()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EquivalentToXml>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.Not.Null);
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.InstanceOf<DateRangeXml>());

        var range = (DateRangeXml)(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range!);
        Assert.That(range.Start, Is.EqualTo(new DateTime(2013,1,1)));
        Assert.That(range.End, Is.EqualTo(new DateTime(2013, 12, 31)));
    }

    [Test]
    public void DeserializeEquivalentTo_PatternIntegerRange_Week1toWeek52()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EquivalentToXml>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.Not.Null);
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.InstanceOf<IIntegerRange>());
        Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range, Is.InstanceOf<IPatternDecorator>());

        var range = (PatternIntegerRangeXml)(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Range!);
        Assert.That(range.Start, Is.EqualTo(1));
        Assert.That(range.End, Is.EqualTo(52));
        Assert.That(range.Step, Is.EqualTo(1));
        Assert.That(range.Pattern, Is.EqualTo("Week "));
        Assert.That(range.Position, Is.EqualTo(PositionValue.Suffix));
    }
}
