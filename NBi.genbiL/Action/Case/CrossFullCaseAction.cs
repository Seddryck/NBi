using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossFullCaseAction : CrossCaseAction
    {
        public CrossFullCaseAction(string firstSet, string secondSet)
            : base(firstSet, secondSet){ }

        
        public override void Execute(GenerationState state)
        {
            state.TestCaseCollection.Cross(FirstSet, SecondSet);
        }

        public override string Display
        {
            get => $"Fully crossing test cases set '{FirstSet}' with '{SecondSet}'";
        }

        
    }
}
