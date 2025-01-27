using NBi.Core.Injection;
using NBi.Core.Scalar.Format;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Format;

public class InvariantFormatterTest
{
    [Test]
    public void Execute_OneGlobalVariable_Processed()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar", new OverridenVariable("myVar", "2018") }
        };
        var formatter = new InvariantFormatter(new ServiceLocator(), new Context(globalVariables));
        var result = formatter.Execute("This year, we are in {@myVar}");
        Assert.That(result, Is.EqualTo("This year, we are in 2018"));
    }

    [Test]
    public void Execute_TwoGlobalVariables_Processed()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar", new OverridenVariable("myVar", "2018") },
            { "myTime", new OverridenVariable("myTime", "YEAR") }
        };
        var formatter = new InvariantFormatter(new ServiceLocator(), new Context(globalVariables));
        var result = formatter.Execute("This {@myTime}, we are in {@myVar}");
        Assert.That(result, Is.EqualTo("This YEAR, we are in 2018"));
    }

    [Test]
    public void Execute_OneGlobalVariablesFormatted_Processed()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar", new OverridenVariable("myVar", new DateTime(2018, 11, 6)) },
        };
        var formatter = new InvariantFormatter(new ServiceLocator(), new Context(globalVariables));
        var result = formatter.Execute("This month is {@myVar:MM}");
        Assert.That(result, Is.EqualTo("This month is 11"));
    }

    [Test]
    [SetCulture("fr-fr")]
    public void ExecuteWithCulture_OneGlobalVariablesFormatted_ProcessedCultureIndepedant()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar", new OverridenVariable("myVar", new DateTime(2018, 8, 6)) },
        };
        var formatter = new InvariantFormatter(new ServiceLocator(), new Context(globalVariables));
        var result = formatter.Execute("This month is {@myVar:MMMM}");
        Assert.That(result, Is.EqualTo("This month is August"));
    }

    [Test]
    public void Execute_OneGlobalVariablesAdvancedFormatted_Processed()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar", new OverridenVariable("myVar", new DateTime(2018, 8, 6)) },
        };
        var formatter = new InvariantFormatter(new ServiceLocator(), new Context(globalVariables));
        var result = formatter.Execute("This month is {@myVar:%M}");
        Assert.That(result, Is.EqualTo("This month is 8"));
    }
}
