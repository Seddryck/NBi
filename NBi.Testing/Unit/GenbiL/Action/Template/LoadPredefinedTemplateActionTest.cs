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
    public class LoadPredefinedTemplateActionTest
    {
        [Test]
        public void Execute_PredefinedTemplate_ValidTemplateCode()
        {
            var resourceName = "ExistsDimension";
            
            var state = new GenerationState();
            var action = new LoadPredefinedTemplateAction(resourceName);
            action.Execute(state);
            Assert.That(state.Template.Code, Is.Not.Null.And.Not.Empty);
        }
    }
}
