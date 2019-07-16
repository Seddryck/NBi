using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    public class MergeCaseAction : IMultiCaseAction
    {
        
        public string MergedScope { get; private set; }
        public MergeCaseAction(string mergedScope)
        {
            MergedScope = mergedScope;
        }

        public void Execute(GenerationState state)
        {
            if (!state.TestCaseCollection.ItemExists(MergedScope))
                throw new ArgumentException(String.Format("Scope '{0}' doesn't exist.", MergedScope));

            var dr = state.TestCaseCollection.Item(MergedScope).Content.CreateDataReader();
            state.TestCaseCollection.CurrentScope.Content.Load(dr, LoadOption.PreserveChanges);
            state.TestCaseCollection.CurrentScope.Content.AcceptChanges();
        }

        public string Display
        {
            get
            {
                return string.Format("Merging with '{0}'", MergedScope);
            }
        }
    }
}
