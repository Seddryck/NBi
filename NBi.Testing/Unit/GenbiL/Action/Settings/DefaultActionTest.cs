using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Settings
{
    public class DefaultActionTest
    {
        [Test]
        public void Execute_InitialValue_ValueIsRecorded()
        {
            var state = new GenerationState();
            var action = new DefaultAction(DefaultType.Assert, "assert-connStr");
            action.Execute(state);

            Assert.That(state.Settings.Defaults[DefaultType.Assert], Is.EqualTo("assert-connStr"));
        }

        [Test]
        public void Execute_ChangeTwice_ValueIsRecorded()
        {
            var state = new GenerationState();
            var action = new DefaultAction(DefaultType.Assert, "assert-connStr");
            action.Execute(state);

            var secondAction = new DefaultAction(DefaultType.Assert, "2nd-assert-connStr");
            secondAction.Execute(state);

            Assert.That(state.Settings.Defaults[DefaultType.Assert], Is.EqualTo("2nd-assert-connStr"));
        }

        [Test]
        public void Execute_ChangeTwoScopes_ValuesAreRecorded()
        {
            var state = new GenerationState();
            var action = new DefaultAction(DefaultType.Assert, "assert-connStr");
            action.Execute(state);

            var secondAction = new DefaultAction(DefaultType.SystemUnderTest, "sut-connStr");
            secondAction.Execute(state);

            Assert.That(state.Settings.Defaults[DefaultType.SystemUnderTest], Is.EqualTo("sut-connStr"));
            Assert.That(state.Settings.Defaults[DefaultType.Assert], Is.EqualTo("assert-connStr"));
        }
    }
}
