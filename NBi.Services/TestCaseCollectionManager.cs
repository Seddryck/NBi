using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Service
{
    public class TestCaseCollectionManager 
    {
        private Dictionary<string, TestCaseManager> dico;
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
            dico = new Dictionary<string, TestCaseManager>();
            connectionStrings = new Dictionary<string, string>();
        }

        public TestCaseManager Item(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = NO_NAME;

            if (!dico.Keys.Contains(name))
                dico.Add(name, new TestCaseManager());

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

        public TestCaseManager Scope
        {
            get
            {
                return Item(scope);
            }
        }

        public void SetFocus(string name)
        {
            if (!dico.Keys.Contains(name))
                dico.Add(name, new TestCaseManager());
            
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

        public void Copy(string from, string to)
        {
            if (!dico.Keys.Contains(from))
                throw new ArgumentException(String.Format("The test case set named '{0}' doesn't exist.", from), "from");

            if (dico.Keys.Contains(to))
                throw new ArgumentException(String.Format("The test case set named '{0}' already exists. The copy command cannot be performed on an existing test cases set", to), "to");

            var dataReader = Item(from).Content.CreateDataReader();

            Item(to).Content.Clear();
            Item(to).Content.Load(dataReader, LoadOption.PreserveChanges);
            Item(to).Content.AcceptChanges();

            Item(to).Variables.Clear();
            foreach (DataColumn col in Item(to).Content.Columns)
                Item(to).Variables.Add(col.ColumnName);
        }
    }
}
