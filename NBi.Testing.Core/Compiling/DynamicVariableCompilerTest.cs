using NBi.Core.Compiling;
using NUnit.Framework;

namespace NBi.Core.Testing.Compiling;

public class DynamicVariableCompilerTest
{
    [Test]
    public void Execute_SimpleCalculation_ExpectedResult()
    {
        using var compiler = new DynamicVariableCompiler();
        compiler.Compile("10*10");
        Assert.That(compiler.Evaluate(), Is.EqualTo(100));
    }
}
