using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.Tolerance;

namespace NBi.Core.Testing.Calculation.Asserting;

public class PredicateReferenceTest
{
    [Test]
    [TestCase(ComparerType.Equal, "A", "A")]
    [TestCase(ComparerType.Equal, "", "(empty)")]
    [TestCase(ComparerType.LessThan, "A", "B")]
    [TestCase(ComparerType.LessThanOrEqual, "A", "B")]
    [TestCase(ComparerType.LessThanOrEqual, "A", "A")]
    [TestCase(ComparerType.MoreThan, "V", "B")]
    [TestCase(ComparerType.MoreThanOrEqual, "V", "B")]
    [TestCase(ComparerType.MoreThanOrEqual, "V", "V")]
    [TestCase(ComparerType.StartsWith, "Paris", "P")]
    [TestCase(ComparerType.EndsWith, "Paris", "s")]
    [TestCase(ComparerType.Contains, "Paris", "ar")]
    [TestCase(ComparerType.MatchesRegex, "Paris", "^[A-Z][a-z]+$")]
    public void Compare_Text_Success(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<string>(y);
        var comparison = StringComparison.InvariantCultureIgnoreCase;
        var predicate = new Mock<CaseSensitivePredicateArgs>(resolver, comparison);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);
        predicate.SetupGet(p => p.StringComparison).Returns(comparison);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.Equal, "A", "(value)")]
    [Ignore("Expressif support for (value) is too light at this moment")]
    public void Compare_SpecialValue_Success(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<string>(y);
        var comparison = StringComparison.InvariantCultureIgnoreCase;
        var predicate = new Mock<CaseSensitivePredicateArgs>(resolver, comparison);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);
        predicate.SetupGet(p => p.StringComparison).Returns(comparison);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [TestCase(ComparerType.StartsWith, "Paris", "p")]
    [TestCase(ComparerType.EndsWith, "Paris", "S")]
    [TestCase(ComparerType.Contains, "Paris", "AR")]
    [TestCase(ComparerType.MatchesRegex, "Paris", "^[A-Z]+$")]
    public void Compare_TextIgnoreCase_Success(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<string>(y);
        var comparison = StringComparison.InvariantCultureIgnoreCase;
        var predicate = new Mock<CaseSensitivePredicateArgs>(resolver, comparison);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);
        predicate.SetupGet(p => p.StringComparison).Returns(comparison);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.Equal, "A", "B")]
    [TestCase(ComparerType.LessThan, "A", "(empty)")]
    [TestCase(ComparerType.LessThanOrEqual, "C", "B")]
    [TestCase(ComparerType.MoreThan, "A", "B")]
    [TestCase(ComparerType.MoreThanOrEqual, "A", "B")]
    [TestCase(ComparerType.StartsWith, "Paris", "p")]
    [TestCase(ComparerType.EndsWith, "Paris", "i")]
    [TestCase(ComparerType.Contains, "Paris", "mar")]
    [TestCase(ComparerType.MatchesRegex, "Paris", "^[A-Z]+$")]
    public void Compare_Text_Failure(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<string>(y);
        var comparison = StringComparison.InvariantCulture;
        var predicate = new Mock<CaseSensitivePredicateArgs>(resolver, comparison);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);
        predicate.SetupGet(p => p.StringComparison).Returns(comparison);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.False);
    }

    [Test]
    public void Compare_TextNull_Success()
    {
        var literal = new LiteralScalarResolver<string>("(null)");
        var args = new Mock<ReferencePredicateArgs>(literal);
        args.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        args.SetupGet(p => p.ComparerType).Returns(ComparerType.Equal);
        args.SetupGet(p => p.Reference).Returns(literal);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(args.Object);
        Assert.That(comparer.Execute(null), Is.True);
    }

    [Test]
    [TestCase(ComparerType.Equal, 1, 1)]
    [TestCase(ComparerType.Equal, 1, 1.0)]
    [TestCase(ComparerType.LessThan, 1, 10)]
    [TestCase(ComparerType.LessThanOrEqual, 1, 10)]
    [TestCase(ComparerType.LessThanOrEqual, 1, "10.0")]
    [TestCase(ComparerType.LessThanOrEqual, 1, 1)]
    [TestCase(ComparerType.MoreThan, 10, 1)]
    [TestCase(ComparerType.MoreThanOrEqual, 10, 1)]
    [TestCase(ComparerType.MoreThanOrEqual, 1, 1)]
    [TestCase(ComparerType.MoreThanOrEqual, 1, "1.00")]
    public void Compare_Numeric_Success(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<decimal>(y);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.Equal, 1, "(value)")]
    [Ignore("Expressif support of (value) is too light at this moment")]
    public void Compare_AnyValue_Success(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<decimal>(y);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.WithinRange, 1, "[1;2]")]
    [TestCase(ComparerType.WithinRange, 1, "(>0)")]
    [TestCase(ComparerType.WithinRange, -1, "(<=-1)")]
    public void Compare_NumericWithinRange_Success(ComparerType comparerType, object x, string range)
    {
        var resolver = new LiteralScalarResolver<string>(range);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [TestCase(ComparerType.WithinRange, 1, "]1;2]")]
    [TestCase(ComparerType.WithinRange, 1, "(<0)")]
    [TestCase(ComparerType.WithinRange, -1, "(>-1)")]
    public void Compare_NumericWithinRange_Failure(ComparerType comparerType, object x, string range)
    {
        var resolver = new LiteralScalarResolver<string>(range);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.False);
    }

    [Test]
    [TestCase(ComparerType.Equal, 1, 1.5)]
    [TestCase(ComparerType.LessThan, 1, -10)]
    [TestCase(ComparerType.LessThanOrEqual, 1, -10)]
    [TestCase(ComparerType.LessThanOrEqual, 1, "-10.0")]
    [TestCase(ComparerType.MoreThan, -10, 1)]
    [TestCase(ComparerType.MoreThanOrEqual, -10, 1)]
    [TestCase(ComparerType.MoreThanOrEqual, -1, 1)]
    [TestCase(ComparerType.MoreThanOrEqual, -1, "1.00")]
    public void Compare_Numeric_Failure(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<decimal>(y);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.False);
    }


    [Test]
    [TestCase(ComparerType.Equal, 1, "(null)")]
    public void Compare_NonNumeric_Exception(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<decimal>(y);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.Throws<ArgumentException>(() => comparer.Execute(x));
    }

    [Test]
    [TestCase(ComparerType.Equal, 10, 10)]
    [TestCase(ComparerType.LessThan, 10, 12)]
    [TestCase(ComparerType.LessThanOrEqual, 10, 12)]
    [TestCase(ComparerType.LessThanOrEqual, 10, 10)]
    [TestCase(ComparerType.MoreThan, 12, 10)]
    [TestCase(ComparerType.MoreThanOrEqual, 12, 10)]
    [TestCase(ComparerType.MoreThanOrEqual, 10, 10)]
    public void Compare_DateTime_Success(ComparerType comparerType, int x, int y)
    {
        var predicate = new Mock<ReferencePredicateArgs>(new LiteralScalarResolver<DateTime>(new DateTime(2015, y, 1)));
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.DateTime);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(new LiteralScalarResolver<DateTime>(new DateTime(2015, y, 1)));

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(new DateTime(2015, x, 1)), Is.True);
    }

    [Test]
    [TestCase("[2015-05-01;2016-05-01[")]
    public void Compare_DateTimeRange_Success(string range)
    {
        var resolver = new LiteralScalarResolver<string>(range);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.DateTime);
        predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.WithinRange);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(new DateTime(2015, 8, 1)), Is.True);
    }

    [Test]
    [TestCase(ComparerType.Equal, true, true)]
    [TestCase(ComparerType.Equal, "true", true)]
    [TestCase(ComparerType.Equal, "Yes", true)]
    public void Compare_Boolean_Success(ComparerType comparerType, object x, object y)
    {
        var resolver = new LiteralScalarResolver<bool>(y);
        var predicate = new Mock<ReferencePredicateArgs>(resolver);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Boolean);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Reference).Returns(resolver);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.LessThan)]
    [TestCase(ComparerType.LessThanOrEqual)]
    [TestCase(ComparerType.MoreThan)]
    [TestCase(ComparerType.MoreThanOrEqual)]
    public void Compare_Boolean_ThrowsArgumentException(ComparerType comparerType)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.Boolean
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        Assert.Throws<ArgumentOutOfRangeException>(delegate { factory.Instantiate(info); });
    }

    [Test]
    public void Compare_NumericVariable_VariableIsEvaluated()
    {
        var variable = new Mock<IVariable>();
        variable.Setup(v => v.GetValue()).Returns(10);
        var variables = new Dictionary<string, IVariable>() { { "var", variable.Object } };
        var global = new GlobalVariableScalarResolver<decimal>("var", new Context(variables));

        var info = new Mock<ReferencePredicateArgs>(global);
        info.SetupGet(i => i.ColumnType).Returns(ColumnType.Numeric);
        info.SetupGet(i => i.ComparerType).Returns(ComparerType.LessThan);
        info.SetupGet(p => p.Reference).Returns(global);

        var factory = new PredicateFactory();
        var predicate = factory.Instantiate(info.Object);
        Assert.That(predicate.Execute(9), Is.True);
        Assert.That(predicate.Execute(11), Is.False);
        variable.Verify(x => x.GetValue(), Times.Exactly(2));
    }

    [Test]
    public void Compare_NumericVariablePartOfDictionary_VariableIsEvaluated()
    {
        var variable = new Mock<IVariable>();
        variable.Setup(v => v.GetValue()).Returns(10);
        var variables = new Dictionary<string, IVariable>() { { "var", variable.Object } };
        var global = new GlobalVariableScalarResolver<decimal>("var", new Context(variables));

        var info = new Mock<ReferencePredicateArgs>(global);
        info.SetupGet(i => i.ColumnType).Returns(ColumnType.Numeric);
        info.SetupGet(i => i.ComparerType).Returns(ComparerType.LessThan);
        info.SetupGet(p => p.Reference).Returns(global);

        var factory = new PredicateFactory();
        var predicate = factory.Instantiate(info.Object);
        Assert.That(predicate.Execute(9), Is.True);
        Assert.That(predicate.Execute(11), Is.False);
        variable.Verify(x => x.GetValue(), Times.Exactly(2));
    }

    [Test]
    public void Compare_NumericVariablePartOfDictionary_PointlessVariableIsNotEvaluated()
    {
        var variableUsed = new Mock<IVariable>();
        variableUsed.Setup(v => v.GetValue()).Returns(10);
        var variablePointless = new Mock<IVariable>();
        variablePointless.Setup(v => v.GetValue()).Returns(0);
        var variables = new Dictionary<string, IVariable>() { { "var", variableUsed.Object }, { "x", variablePointless.Object } };
        var global = new GlobalVariableScalarResolver<decimal>("var", new Context(variables));

        var info = new Mock<ReferencePredicateArgs>(global);
        info.SetupGet(i => i.ColumnType).Returns(ColumnType.Numeric);
        info.SetupGet(i => i.ComparerType).Returns(ComparerType.LessThan);
        info.SetupGet(p => p.Reference).Returns(global);

        var factory = new PredicateFactory();
        var predicate = factory.Instantiate(info.Object);
        Assert.That(predicate.Execute(9), Is.True);
        Assert.That(predicate.Execute(11), Is.False);
        variablePointless.Verify(x => x.GetValue(), Times.Never);
    }
}
