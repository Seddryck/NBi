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
    public class ParallelizeQueriesActionTest
    {
        [Test]
        public void Execute_TrueValue_ValueIsRecorded()
        {
            var state = new GenerationState();
            var action = new ParallelizeQueriesAction(true);
            action.Execute(state);

            Assert.That(state.Settings.ParallelizeQueries, Is.True);
        }

        [Test]
        public void Execute_FalseValue_ValueIsRecorded()
        {
            var state = new GenerationState();
            var action = new ParallelizeQueriesAction(false);
            action.Execute(state);

            Assert.That(state.Settings.ParallelizeQueries, Is.False);
        }
    }
}
