using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using NUnit.Framework;

namespace NBi.GenbiL.Action.Template
{
    public class ListPredefinedTemplateActionTest
    {
        [Test]
        public void List_None_ValidList()
        {
            var state = new GenerationState();
            var action = new ListPredefinedTemplateAction();
            action.Execute(state);
            Assert.That(state.Template.PredefinedLabels, Has.Some.EqualTo("Exists Dimension"));
            Assert.That(state.Template.PredefinedLabels.Count(), Is.GreaterThan(10));
        }
    }
}
