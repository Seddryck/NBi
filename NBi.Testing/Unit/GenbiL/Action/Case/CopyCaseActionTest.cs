using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class CopyCaseActionTest
    {
        private GenerationState BuildOriginalState()
        {
            var state = new GenerationState();
            var master = state.TestCaseCollection.Item("master");
            master.Content.Columns.Add("keyColumn1");
            master.Content.Columns.Add("keyColumn2");
            master.Content.Columns.Add("thirdColumn");
            master.Variables.Add("keyColumn1");
            master.Variables.Add("keyColumn2");
            master.Variables.Add("thirdColumn");
            var firstAlphaRow = master.Content.NewRow();
            firstAlphaRow[0] = "key1";
            firstAlphaRow[1] = "keyA";
            firstAlphaRow[2] = "thirdAlphaCell1";
            master.Content.Rows.Add(firstAlphaRow);
            var secondAlphaRow = master.Content.NewRow();
            secondAlphaRow[0] = "key2";
            secondAlphaRow[1] = "keyB";
            secondAlphaRow[2] = "thirdAlphaCell2";
            master.Content.Rows.Add(secondAlphaRow);

            return state;
        }

        [Test]
        public void Copy_SimpleMaster_CopyIsEffectivelyDone()
        {
            var state = BuildOriginalState();
            var master = state.TestCaseCollection.Item("master");

            var action = new CopyCaseAction("master", "copied");
            action.Execute(state);

            Assert.That(state.TestCaseCollection.ItemExists("copied"));
            var copied = state.TestCaseCollection.Item("copied");

            for (int i = 0; i < master.Content.Rows.Count; i++)
                Assert.That(copied.Content.Rows[i].ItemArray, Is.EqualTo(master.Content.Rows[i].ItemArray));

            Assert.That(copied.Content.Rows, Has.Count.EqualTo(master.Content.Rows.Count));
        }


        [Test]
        public void Copy_SimpleMaster_CopyIsNotReferenceCopy()
        {
            var state = BuildOriginalState();
            var master = state.TestCaseCollection.Item("master");

            var action = new CopyCaseAction("master", "copied");
            action.Execute(state);
            var copied = state.TestCaseCollection.Item("copied");
            master.Content.Clear();

            Assert.That(master.Content.Rows, Has.Count.EqualTo(0));
            Assert.That(copied.Content.Rows, Has.Count.GreaterThan(0));
        }

        [Test]
        public void Copy_SimpleMasterWithCopiedAlreadyLoaded_CopyIsNotAllowed()
        {
            var state = BuildOriginalState();
            var master = state.TestCaseCollection.Item("master");

            var copied = state.TestCaseCollection.Item("copied");

            var action = new CopyCaseAction("master", "copied");

            Assert.Throws<ArgumentException>(delegate { action.Execute(state); });
        }

    }
}
