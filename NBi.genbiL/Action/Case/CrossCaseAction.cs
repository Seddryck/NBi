using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    abstract class CrossCaseAction : ICaseAction
    {
        public string FirstSet { get; private set; }
        public string SecondSet { get; private set; }

        public CrossCaseAction(string firstSet, string secondSet)
        {
            FirstSet = firstSet;
            SecondSet = secondSet;
        }

        public abstract void Execute(GenerationState state);
        public abstract string Display { get; }
    }
}
