using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful
{
    public class TestCaseSetCollectionState
    {
        private Dictionary<string, TestCaseSetState> dico;
        private string scope;
        private const string NO_NAME = "_noname";
        public string CurrentScopeName
        {
            get
            {
                return scope;
            }
        }

        public TestCaseSetCollectionState()
        {
            dico = new Dictionary<string, TestCaseSetState>();
        }

        public TestCaseSetState Item(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = NO_NAME;

            if (!dico.Keys.Contains(name))
                dico.Add(name, new TestCaseSetState());

            if (dico.Count == 1)
                scope = name;

            return dico[name];
        }

        public bool ItemExists(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = NO_NAME;

            return dico.Keys.Contains(name);
        }

        public TestCaseSetState Scope
        {
            get
            {
                return Item(scope);
            }
        }

        public void SetFocus(string name)
        {
            if (!dico.Keys.Contains(name))
                dico.Add(name, new TestCaseSetState());

            scope = name;
        }
    }
}
