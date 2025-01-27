using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;
using NBi.GenbiL.Action.Variable;

namespace NBi.GenbiL.Testing.Parser;

[TestFixture]
public class VariableParserTest
{
    [Test]
    public void SentenceParser_Include_IncludeAction()
    {
        var input = "variable include file 'variables.xml';";
        var result = Variable.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IncludeVariableAction>());
        Assert.That(((IncludeVariableAction)result).Filename, Is.EqualTo("variables.xml"));
    }
}
