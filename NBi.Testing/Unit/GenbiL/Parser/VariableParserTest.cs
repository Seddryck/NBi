using System;
using System.Linq;
using NBi.GenbiL.Action.Variable;
using NBi.GenbiL.Parser;
using NBi.Service;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class VariableParserTest
    {
        [Test]
        public void SentenceParser_VariableSet_ValidAction()
        {
            var input = "variable set 'minDate' to '2010-10-10';";
            var result = Variable.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<SetVariableAction>());
            Assert.That(((SetVariableAction)result).Name, Is.EqualTo("minDate"));
            Assert.That(((SetVariableAction)result).Value, Is.EqualTo("2010-10-10"));
        }
        
    }
}
