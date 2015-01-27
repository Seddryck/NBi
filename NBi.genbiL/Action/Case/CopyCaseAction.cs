using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
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
            if (!state.TestCaseSetCollection.ItemExists(From))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", From), "from");

            if (state.TestCaseSetCollection.ItemExists(To))
                throw new ArgumentException(String.Format("The test case set named '{0}' already exists. The copy command cannot be performed on an existing test cases set", To), "to");

            var dataReader = state.TestCaseSetCollection.Item(From).Content.CreateDataReader();

            var target = state.TestCaseSetCollection.Item(To);
            target.Content.Clear();
            target.Content.Load(dataReader, LoadOption.PreserveChanges);
            target.Content.AcceptChanges();
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Copying set of test cases from '{0}' to '{1}'", From, To);
            }
        }
    }
}
