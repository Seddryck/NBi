using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Comparer;

public class NumericToleranceFactoryTest
{
    [Test]
    public void Instantiate_Absolute_Value()
    {
        var value = "50000";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericAbsoluteTolerance>());
        Assert.That(tolerance.Value, Is.EqualTo(50000));
        Assert.That(tolerance.ValueString, Is.EqualTo("50000"));
    }

    [Test]
    public void Instantiate_AbsoluteWithDecimalSeparator_Value()
    {
        var value = "50.250";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericAbsoluteTolerance>());
        Assert.That(tolerance.Value, Is.EqualTo(50.25));
        Assert.That(tolerance.ValueString, Is.EqualTo("50.250"));
    }

    [Test]
    public void Instantiate_Percentage_Value()
    {
        var value = "50%";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericPercentageTolerance>());
        Assert.That(tolerance.Value, Is.EqualTo(0.5));
        Assert.That(tolerance.ValueString, Is.EqualTo("50.0%"));
    }

    [Test]
    public void Instantiate_BoundedPercentage_Value()
    {
        var value = "50% (min 0.001)";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericBoundedPercentageTolerance>());
        var boundedTolerance = (NumericBoundedPercentageTolerance)tolerance;

        Assert.That(boundedTolerance.Value, Is.EqualTo(0.5));
        Assert.That(boundedTolerance.Min, Is.EqualTo(0.001));
        Assert.That(boundedTolerance.Max, Is.EqualTo(0));
        Assert.That(boundedTolerance.ValueString, Is.EqualTo("50.0% (min: 0.001)"));
    }

    [Test]
    public void Instantiate_BoundedPercentageWithSpaceAndWithoutBrackets_Value()
    {
        var value = " 50 % min:0.001 ";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericBoundedPercentageTolerance>());
        var boundedTolerance = (NumericBoundedPercentageTolerance)tolerance;

        Assert.That(boundedTolerance.Value, Is.EqualTo(0.5));
        Assert.That(boundedTolerance.Min, Is.EqualTo(0.001));
        Assert.That(boundedTolerance.Max, Is.EqualTo(0));
        Assert.That(boundedTolerance.ValueString, Is.EqualTo("50.0% (min: 0.001)"));
    }

    [Test]
    public void Instantiate_BoundedPercentageWithEqual_Value()
    {
        var value = "10%(max=125) ";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericBoundedPercentageTolerance>());
        var boundedTolerance = (NumericBoundedPercentageTolerance)tolerance;

        Assert.That(boundedTolerance.Value, Is.EqualTo(0.1));
        Assert.That(boundedTolerance.Min, Is.EqualTo(0));
        Assert.That(boundedTolerance.Max, Is.EqualTo(125));
        Assert.That(boundedTolerance.ValueString, Is.EqualTo("10.0% (max: 125)"));
    }

    [Test]
    public void Instantiate_OneSidedMore_Value()
    {
        var value = " + 50%";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericPercentageTolerance>());
        var boundedTolerance = (NumericPercentageTolerance)tolerance;

        Assert.That(boundedTolerance.Value, Is.EqualTo(0.5));
        Assert.That(boundedTolerance.ValueString, Is.EqualTo("+50.0%"));
    }

    [Test]
    public void Instantiate_OneSidedLess_Value()
    {
        var value = "-16.25";
        var tolerance = new NumericToleranceFactory().Instantiate(value);
        Assert.That(tolerance, Is.TypeOf<NumericAbsoluteTolerance>());
        var boundedTolerance = (NumericAbsoluteTolerance)tolerance;

        Assert.That(boundedTolerance.Value, Is.EqualTo(16.25));
        Assert.That(boundedTolerance.ValueString, Is.EqualTo("-16.25"));
    }
}
