using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Variable
{
    public class AutoVariableActionTest
    {
        [Test]
        [TestCase("fr-fr")]
        [TestCase("nl-be")]
        [TestCase("fr-be")]
        [TestCase("de-de")]
        [TestCase("en-us")]
        [TestCase("en-gb")]
        public void Execute_CultureIndependant_VariableNow(string culture)
        {
            var state = new GenerationState();
            state.Variables.Clear();

            var action = new AutoVariableAction(true, new DateTime(2014, 09, 26, 9, 16, 55));
            action.Execute(state);

            Assert.That(state.Variables["now"], Is.EqualTo("2014-09-26T09:16:55"));
        }

        [Test]
        [TestCase("fr-fr")]
        [TestCase("nl-be")]
        [TestCase("fr-be")]
        [TestCase("de-de")]
        [TestCase("en-us")]
        [TestCase("en-gb")]
        public void Execute_CultureIndependant_VariableTime(string culture)
        {
            var state = new GenerationState();
            state.Variables.Clear();

            var action = new AutoVariableAction(true, new DateTime(2014, 09, 26, 9, 16, 55));
            action.Execute(state);

            Assert.That(DateTime.Parse(state.Variables["time"].ToString()).TimeOfDay, Is.EqualTo(new TimeSpan(9, 16, 55)));
        }

        [Test]
        [TestCase("fr-fr")]
        [TestCase("nl-be")]
        [TestCase("fr-be")]
        [TestCase("de-de")]
        [TestCase("en-us")]
        [TestCase("en-gb")]
        public void Execute_CultureIndependant_VariableDate(string culture)
        {
            var state = new GenerationState();
            state.Variables.Clear();

            var action = new AutoVariableAction(true, new DateTime(2014, 09, 26, 9, 16, 55));
            action.Execute(state);

            Assert.That(DateTime.Parse(state.Variables["today"].ToString()).Date, Is.EqualTo(new DateTime(2014, 09, 26)));
        }
        
    }
}
