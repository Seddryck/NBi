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
    public class ReferenceActionTest
    {
        [Test]
        public void Execute_InitialValue_ValueIsRecorded()
        {
            var state = new GenerationState();
            var action = new ReferenceAction("reference 1", "ref1-connStr");
            action.Execute(state);

            Assert.That(state.Settings.References, Has.Count.EqualTo(1));
            Assert.That(state.Settings.References["reference 1"], Is.Not.Null.Or.Empty);
            Assert.That(state.Settings.References["reference 1"], Is.EqualTo("ref1-connStr"));
        }

        [Test]
        public void Execute_ChangeTwice_ValueIsRecorded()
        {
            var state = new GenerationState();
            var action = new ReferenceAction("reference 1", "ref1-connStr");
            action.Execute(state);

            var action2 = new ReferenceAction("reference 1", "ref1-connStr-newValue");
            action2.Execute(state);

            Assert.That(state.Settings.References, Has.Count.EqualTo(1));
            Assert.That(state.Settings.References["reference 1"], Is.Not.Null.Or.Empty);
            Assert.That(state.Settings.References["reference 1"], Is.EqualTo("ref1-connStr-newValue"));
        }

        [Test]
        public void Execute_ChangeTwoReferences_ValuesAreRecorded()
        {
            var state = new GenerationState();
            var action = new ReferenceAction("reference 1", "ref1-connStr");
            action.Execute(state);

            var action2 = new ReferenceAction("reference 2", "ref2-connStr");
            action2.Execute(state);

            Assert.That(state.Settings.References, Has.Count.EqualTo(2));
            Assert.That(state.Settings.References["reference 1"], Is.EqualTo("ref1-connStr"));
            Assert.That(state.Settings.References["reference 2"], Is.EqualTo("ref2-connStr"));
        }
    }
}
