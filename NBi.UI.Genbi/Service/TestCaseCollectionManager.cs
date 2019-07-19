using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.UI.Genbi.Service
{
    public class TestCaseCollectionManager 
    {
        private readonly Dictionary<string, CaseSet> dico;
        private string scope;
        private const string NO_NAME = "_noname";
        public string CurrentScopeName
        {
            get
            {
                return scope;
            }
        }

        public TestCaseCollectionManager()
        {
            dico = new Dictionary<string, CaseSet>();
            connectionStrings = new Dictionary<string, string>();
        }

        public CaseSet Item(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = NO_NAME;

            if (!dico.Keys.Contains(name))
                dico.Add(name, new CaseSet());

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

        private readonly Dictionary<string, string> connectionStrings;
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                return connectionStrings;
            }
        }

        public List<string> ConnectionStringNames
        {
            get
            {
                return ConnectionStrings.Keys.ToList();
            }
        }

        public CaseSet CurrentScope => Item(scope);

        public void SetFocus(string name)
        {
            if (!dico.Keys.Contains(name))
                dico.Add(name, new CaseSet());
            
            scope = name;
        }

        public void AddConnectionStrings(string name, string value)
        {
            if (connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings.Add(name, value);
        }

        public void RemoveConnectionStrings(string name)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings.Remove(name);
        }

        public void EditConnectionStrings(string name, string newValue)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings[name] = newValue;
        }
    }
}
