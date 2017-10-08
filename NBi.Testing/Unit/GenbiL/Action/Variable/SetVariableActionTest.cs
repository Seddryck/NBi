using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Variable
{
    public class SetVariableActionTest
    {
        [Test]
        public void Execute_NewVariable_VariableAdded()
        {
            var state = new GenerationState();
            state.Variables.Clear();
            
            var action = new SetVariableAction("myVar", "2010-10-10");
            action.Execute(state);
            Assert.That(state.Variables, Has.Count.EqualTo(1));
            Assert.That(state.Variables.Keys, Has.Member("myVar"));
            Assert.That(state.Variables["myVar"], Is.EqualTo("2010-10-10"));
        }

        [Test]
        public void Execute_ExistingVariable_VariableUpdated()
        {
            var state = new GenerationState();
            state.Variables.Clear();
            state.Variables.Add("myVar", "2012-12-12");

            var action = new SetVariableAction("myVar", "2010-10-10");
            action.Execute(state);
            Assert.That(state.Variables, Has.Count.EqualTo(1));
            Assert.That(state.Variables.Keys, Has.Member("myVar"));
            Assert.That(state.Variables["myVar"], Is.EqualTo("2010-10-10"));
        }
    }
}
