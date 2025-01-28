using System;
using System.Linq;
using NBi.GenbiL.Action.Consumable;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.GenbiL.Testing.Parser;

[TestFixture]
public class ConsumableParserTest
{
    [Test]
    public void SentenceParser_ConsumableSet_ValidAction()
    {
        var input = "consumable set 'minDate' to '2010-10-10';";
        var result = Consumable.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<SetConsumableAction>());
        Assert.That(((SetConsumableAction)result).Name, Is.EqualTo("minDate"));
        Assert.That(((SetConsumableAction)result).Value, Is.EqualTo("2010-10-10"));
    }
    
}
