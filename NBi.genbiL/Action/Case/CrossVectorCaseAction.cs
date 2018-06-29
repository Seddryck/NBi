using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossVectorCaseAction : CrossCaseAction
    {
        public IEnumerable<string> Values { get; set; }

        public CrossVectorCaseAction(string firstSet, string vectorName, IEnumerable<string> values)
            : base(firstSet, vectorName)
        {
            Values = values;
        }

        public override void Execute(GenerationState state)
        {
            state.TestCaseCollection.Cross(FirstSet, SecondSet, Values);
        }

        public override string Display
        {
            get => $"Crossing test cases set '{FirstSet}' with vector '{SecondSet}' defined as '{String.Join("', '", Values)}'";
        }

        
    }
}
