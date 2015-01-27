using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using NUnit.Framework;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Template
{
    public class LoadExternalTemplateActionTest
    {
        [Test]
        public void Execute_ExternalTemplate_ValidTemplateCode()
        {
            var path = "ExternalTemplate.nbits";
            var templateText = "<test name=\"$name$\"/>";
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, templateText);
            
            var state = new GenerationState();
            var action = new LoadExternalTemplateAction(path);
            action.Execute(state);
            Assert.That(state.Template.Code, Is.EqualTo(templateText));
        }
    }
}
