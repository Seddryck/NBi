using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Stateful;
using NBi.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Suite
{
    public class SaveSuiteCaseActionTest
    {

        [Test]
        public void Execute_SaveFile_ContentPersisted()
        {
            var state = new GenerationState();
            state.Suite.Tests.Add(new TestXml());

            var inMemory = new MemoryStream();
            var streamWriter = new StreamWriter(inMemory);

            var action = new SaveSuiteAction(streamWriter);
            action.Execute(state);
            Assert.That(inMemory.Position, Is.GreaterThan(0));
        }

        [Test]
        public void Execute_SaveFileTwice_SecondContentGreaterThanFirst()
        {
            var state = new GenerationState();
            state.Suite.Tests.Add(new TestXml() { Name = "First Test" });

            long firstLength = 0;
            using (var inMemory = new MemoryStream())
            {
                var streamWriter = new StreamWriter(inMemory);

                var action = new SaveSuiteAction(streamWriter);
                action.Execute(state);
                firstLength = inMemory.Position;

            }

            state.Suite.Tests.Add(new TestXml() {Name="Second Test"});
            long secondLength = 0;
            using (var inMemory = new MemoryStream())
            {
                var streamWriter = new StreamWriter(inMemory);

                var action = new SaveSuiteAction(streamWriter);
                action.Execute(state);
                secondLength = inMemory.Position;
            }


            Assert.That(secondLength, Is.GreaterThan(firstLength));
        }

    }
}
