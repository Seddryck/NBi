using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Stateful;
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
    public class IncludeSuiteCaseActionTest
    {
        private string FirstTestXml
        {
            get
            {
                return
                    "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<test name=\"In dimension Customers, the members of level named 'State-Provinces' contain 'Washington'\">" +
                    "   <system-under-test>" +
                    "        <members>" +
                    "	        <level caption=\"State-Province\" dimension=\"Customer\" hierarchy=\"State-Province\" perspective=\"Adventure Works\"/>" +
                    "        </members>" +
                    "    </system-under-test>" +
                    "    <assert>" +
                    "        <contain caption=\"Washington\"/>" +
                    "    </assert>" +
                    "</test>";
            }
        }

        private string SecondTestXml
        {
            get
            {
                return
                    "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                    "<test name=\"In dimension Customers, the members of level named 'State-Provinces' doesn't contain 'Hainaut'\">" +
                    "   <system-under-test>" +
                    "        <members>" +
                    "	        <level caption=\"State-Province\" dimension=\"Customer\" hierarchy=\"State-Province\" perspective=\"Adventure Works\"/>" +
                    "        </members>" +
                    "    </system-under-test>" +
                    "    <assert not=\"true\">" +
                    "        <contain caption=\"Hainaut\"/>" +
                    "    </assert>" +
                    "</test>";
            }
        }

        [Test]
        public void Execute_IncludeFile_FileIncluded()
        {
            var state = new GenerationState();

            TextReader streamReader = new StringReader(FirstTestXml);

            var action = new IncludeSuiteAction(streamReader);
            action.Execute(state);
            Assert.That(state.Suite.Tests, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute_IncludeFileTwice_FilesIncluded()
        {
            var state = new GenerationState();

            TextReader firstReader = new StringReader(FirstTestXml);
            var firstAction = new IncludeSuiteAction(firstReader);
            firstAction.Execute(state);
            Assert.That(state.Suite.Tests, Has.Count.EqualTo(1));

            TextReader secondReader = new StringReader(FirstTestXml);
            var secondAction = new IncludeSuiteAction(secondReader);
            secondAction.Execute(state);
            Assert.That(state.Suite.Tests, Has.Count.EqualTo(2));
        }
    }
}
