using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    class CrossVectorCaseAction : CrossAbstractCaseAction
    {
        public string VectorName { get; set; }
        public IEnumerable<string> Values { get; set; }

        public CrossVectorCaseAction(string firstSet, string vectorName, IEnumerable<string> values)
            :base(firstSet)
        {
            VectorName = vectorName;
            Values = values;
        }

        protected override IDataReader CrossContent(GenerationState state)
        {
            var vector = new DataTable();
            vector.Columns.Add(VectorName);
            foreach (var item in Values)
            {
                var row = vector.NewRow();
                row.ItemArray = new[] { item };
                vector.Rows.Add(row);
            }

            return CrossContent(
                state.TestCaseSetCollection.Item(FirstSet).Content
                , vector
                , delegate { return true; } 
            );
        }

        public override string Display
        {
            get
            {
                return string.Format("Crossing test cases set '{0}' with vector '{1}' defined as '{2}'", FirstSet, VectorName, String.Join("', '", Values));
            }
        }

        
    }
}
