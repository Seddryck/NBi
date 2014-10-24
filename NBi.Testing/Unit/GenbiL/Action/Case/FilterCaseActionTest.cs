﻿using NBi.GenbiL.Action.Case;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class FilterCaseActionTest
    {
        [Test]
        public void Display_LikeOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", NBi.Service.Operator.Like, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances like 'first value'"));
        }

        [Test]
        public void Display_NotLikeOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", NBi.Service.Operator.Like, new[] { "first value" }, true);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances not like 'first value'"));
        }

        [Test]
        public void Display_EqualOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", NBi.Service.Operator.Equal, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances equal to 'first value'"));
        }

        [Test]
        public void Display_LikeMultipleValues_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", NBi.Service.Operator.Like, new[] { "first value", "second value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances like 'first value', 'second value'"));
        }
    }
}
