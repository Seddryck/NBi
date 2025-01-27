using NBi.Core.Compiling;
using NUnit.Framework;

namespace NBi.Core.Testing.Compiling;

public class TransformerCompilerTest
{
    [Test]
    public void Execute_SimpleCalculation_ExpectedResult()
    {
        using var compiler = new TransformerCompiler<int>();
        compiler.Compile("10*value");
        Assert.That(compiler.Evaluate(5), Is.EqualTo(50));
    }

    [Test]
    public void Execute_TernaryOperator_ExpectedResult()
    {
        using var compiler = new TransformerCompiler<int>();
        compiler.Compile("value % 2 == 0 ? \"Foo\" : \"Bar\" ");
        Assert.That(compiler.Evaluate(4), Is.EqualTo("Foo"));
        Assert.That(compiler.Evaluate(5), Is.EqualTo("Bar"));
    }
}
