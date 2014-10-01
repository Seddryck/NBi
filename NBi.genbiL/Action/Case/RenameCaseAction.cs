﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    public class RenameCaseAction : ICaseAction
    {
        public string OldVariableName { get; set; }
        public string NewVariableName { get; set; }
        public RenameCaseAction(string oldVariableName, string newVariableName)
        {
            OldVariableName = oldVariableName;
            NewVariableName = newVariableName;
        }

        public void Execute(GenerationState state)
        {
            var index = state.TestCaseCollection.Focus.Variables.ToList().FindIndex(v => v == OldVariableName);
            state.TestCaseCollection.Focus.Variables[index] = NewVariableName;
            state.TestCaseCollection.Focus.Content.Columns[index].ColumnName = NewVariableName;
        }

        public string Display
        {
            get
            {
                return string.Format("Renaming column '{0}' into '{1}'", OldVariableName, NewVariableName);
            }
        }
    }
}
