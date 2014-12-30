using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CopyCaseAction : ICaseAction
    {
        public string From { get; set; }
        public string To { get; set; }

        public CopyCaseAction(string from, string to)
        {
            From = from;
            To = to;
        }

        public void Execute(GenerationState state)
        {
            state.TestCaseCollection.Copy(From, To);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Copying test cases set '{0}' to '{1}'", From, To);
            }
        }
    }
}
