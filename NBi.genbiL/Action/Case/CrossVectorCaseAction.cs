using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossVectorCaseAction : ICaseAction
    {
        public string FirstSet { get; set; }
        public string VectorName { get; set; }
        public IEnumerable<string> Values { get; set; }

        public CrossVectorCaseAction(string firstSet, string vectorName, IEnumerable<string> values)
        {
            FirstSet = firstSet;
            VectorName = vectorName;
            Values = values;
        }

        public void Execute(GenerationState state)
        {
            state.TestCaseCollection.Cross(FirstSet, VectorName, Values);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Crossing test cases set '{0}' with vector '{1}' defined as '{2}'", FirstSet, VectorName, String.Join("', '", Values));
            }
        }

        
    }
}
