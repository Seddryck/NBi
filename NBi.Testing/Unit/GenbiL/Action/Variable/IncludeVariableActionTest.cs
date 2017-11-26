using NBi.Core.Transformation;
using NBi.GenbiL;
using NBi.GenbiL.Action.Variable;
using NBi.Xml.Variables;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Variable
{
    [TestFixture]
    public class IncludeVariableActionTest
    {
        public class IncludeVariableActionTestable : IncludeVariableAction
        {
            private Stream stream;
            public IncludeVariableActionTestable(Stream stream)
                : base("memoryStream.io")
            {
                this.stream = stream;
            }

            protected override IEnumerable<GlobalVariableXml> ReadXml(string filename)
            {
                return ReadXml(stream);
            }
        }

        [Test]
        public void Execute_Filename_StateUpdated()
        {
            var state = new GenerationState();

            using (var memory = new MemoryStream())
            {
                var sw = new StreamWriter(memory, new UnicodeEncoding());
                sw.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
                            <variables>
                              <variable name=""var1"">
                                <script language=""c-sharp"">DateTime.Now</script>
                              </variable>
                              <variable name=""var2"">
                                <script language=""c-sharp"">DateTime.Now.Year+1</script>
                              </variable>
                            </variables> ");
                sw.Flush();
                memory.Seek(0, SeekOrigin.Begin);

                var action = new IncludeVariableActionTestable(memory);
                action.Execute(state);
            }

            Assert.That(state.Variables, Has.Count.EqualTo(2));
            Assert.That(state.Variables.ContainsKey("var1"), Is.True);
            Assert.That(state.Variables.ContainsKey("var2"), Is.True);
            Assert.That(state.Variables.All(x => (x.Value as GlobalVariableXml).Script.Language==LanguageType.CSharp), Is.True);
            Assert.That(state.Variables.All(x => (x.Value as GlobalVariableXml).Script.Code.StartsWith("DateTime")), Is.True);
        }
    }
}
