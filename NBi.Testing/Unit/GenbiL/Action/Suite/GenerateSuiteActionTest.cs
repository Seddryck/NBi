using NBi.GenbiL;
using NBi.GenbiL.Action.Suite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Suite
{
    
    public class GenerateSuiteActionTest
    {
        protected GenerationState BuildInitialState(object obj)
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("one");
            state.TestCaseCollection.Scope.Content.Columns.Add("two", typeof(object));
            state.TestCaseCollection.Scope.Variables.Add("one");
            state.TestCaseCollection.Scope.Variables.Add("two");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "a";
            firstRow[1] = obj;
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            state.Template.Code = "<test name='$one$ + $two$'/>";
            return state;
        }

        [Test]
        public void Execute_SimpleDataTable_Rendered()
        {
            var state = BuildInitialState("b");
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(1));
            var test = state.List.Tests[0];
            Assert.That(test.Name, Is.EqualTo("a + b"));
        }

        [Test]
        public void Execute_SimpleDataTable2_Rendered()
        {
            var state = BuildInitialState("foo");
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(1));
            var test = state.List.Tests[0];
            Assert.That(test.Name, Is.EqualTo("a + foo"));
        }

        [Test]
        public void Execute_ComplexDataTable_Rendered()
        {
            var state = BuildInitialState(new [] {"b", "c"});
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(1));
            var test = state.List.Tests[0];
            Assert.That(test.Name, Is.EqualTo("a + bc"));
        }
    }
}
