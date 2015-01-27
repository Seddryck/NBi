using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    public class MergeCaseAction : ICaseAction
    {
        
        public string MergedScope { get; private set; }
        public MergeCaseAction(string mergedScope)
        {
            MergedScope = mergedScope;
        }

        public void Execute(GenerationState state)
        {
            if (!state.TestCaseSetCollection.ItemExists(MergedScope))
                throw new ArgumentException(String.Format("Scope '{0}' doesn't exist.", MergedScope));

            var dr = state.TestCaseSetCollection.Item(MergedScope).Content.CreateDataReader();
            state.TestCaseSetCollection.Scope.Content.Load(dr, LoadOption.PreserveChanges);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();
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
