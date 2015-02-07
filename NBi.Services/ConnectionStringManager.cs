using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Service
{
    public class ConnectionStringManager
    {
        private readonly IDictionary<string, string> connectionStrings;

        public ConnectionStringManager()
        {
            connectionStrings = new Dictionary<string, string>();
        }

        public void Add(string name, string value)
        {
            if (connectionStrings.Keys.Contains(name))
                throw new ArgumentException();

            connectionStrings.Add(name, value);
        }

        public void Remove(string name)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException();

            connectionStrings.Remove(name);
        }

        public void Edit(string name, string newValue)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException();

            connectionStrings[name] = newValue;
        }

        public string Get(string name)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException();

            return connectionStrings[name];
        }

        public IEnumerable<string> Names
        {
            get
            {
                return connectionStrings.Keys;
            }
        }
    }
}
