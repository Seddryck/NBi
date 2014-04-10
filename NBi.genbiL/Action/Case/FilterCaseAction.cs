using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case
{
    abstract class FilterCaseAction : ICaseAction
    {
        public string Text { get; set; }
        public string Column { get; set; }

        public FilterCaseAction(string text, string column)
        {
            Text = text;
            Column = column;
        }
        public abstract void Execute(GenerationState state);

        public virtual string Display
        {
            get
            {
                // TODO: Implement this property getter
                throw new NotImplementedException();
            }
        }
    }
}
