using NBi.Core.Scalar;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar;

public class PercentageTest
{
    [Test]
    [TestCase("0.4%", 0.004)]
    [TestCase("40.0%", 0.4)]
    public void Convert_FromStringWithPercentage_Correct(string value, double result)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Percentage));
        Assert.That(converter.CanConvertFrom(value.GetType()));
        var pc = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc, Is.TypeOf<Percentage>());
        Assert.That(((Percentage)pc!).Value, Is.EqualTo(result));
    }

    [Test]
    [TestCase("0.4", 0.4)]
    [TestCase("40", 40)]
    public void Convert_FromStringWithoutPercentage_CorrectValue(string value, double result)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Percentage));
        Assert.That(converter.CanConvertFrom(value.GetType()));
        var pc = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc, Is.TypeOf<Percentage>());
        Assert.That(((Percentage)pc!).Value, Is.EqualTo(result));
    }

    [Test]
    [SetCulture("en-us")]
    [TestCase("0.4", "40%")]
    [TestCase("40", "4000%")]
    [TestCase("50%", "50%")]
    [TestCase("0.500%", "0.5%")]
    public void Convert_FromStringWithoutPercentageEnglish_CorrectString(string value, string result)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Percentage));
        Assert.That(converter.CanConvertFrom(value.GetType()));
        var pc = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc, Is.TypeOf<Percentage>());
        Assert.That(((Percentage)pc!).ToString(), Is.EqualTo(result));
    }

    [Test]
    [SetCulture("fr-fr")]
    [TestCase("0.4", "40%")]
    [TestCase("40", "4000%")]
    [TestCase("50%", "50%")]
    [TestCase("0.500%", "0.5%")]
    public void Convert_FromStringWithoutPercentageFrench_CorrectString(string value, string result)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Percentage));
        Assert.That(converter.CanConvertFrom(value.GetType()));
        var pc = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc, Is.TypeOf<Percentage>());
        Assert.That(((Percentage)pc!).ToString(), Is.EqualTo(result));
    }

    [Test]
    [SetCulture("ar-SA")]
    [TestCase("0.4", "40%")]
    [TestCase("40", "4000%")]
    [TestCase("50%", "50%")]
    [TestCase("0.500%", "0.5%")]
    public void Convert_FromStringWithoutPercentageArabic_CorrectString(string value, string result)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Percentage));
        Assert.That(converter.CanConvertFrom(value.GetType()));
        var pc = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc, Is.TypeOf<Percentage>());
        Assert.That(((Percentage)pc!).ToString(), Is.EqualTo(result));
    }
}
