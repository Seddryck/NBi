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
