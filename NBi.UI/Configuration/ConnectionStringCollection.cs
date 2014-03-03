using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Configuration
{
    public class ConnectionStringCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return _dico.GetEnumerator();
        }

       
        public enum ConnectionType
        {
            Expect,
            Actual
        }

        public enum ConnectionClass
        {
            Adomd,
            Oledb
        }

        public struct ConnectionDefinition
        {
            public ConnectionType Type;
            public ConnectionClass Class;

            public ConnectionDefinition(ConnectionClass clazz, ConnectionType type)
            {
                Type= type;
                Class=clazz;
            }

        }
        
        public string Root { get; set; }
        protected Dictionary<ConnectionDefinition, ConnectionString> _dico;

        public ConnectionStringCollection()
        {
            _dico = new Dictionary<ConnectionDefinition, ConnectionString>();
        }

        public ConnectionString this[ConnectionDefinition def]
        {
            get
            {
                if (!_dico.ContainsKey(def))
                    _dico[def] = new ConnectionString();
                return _dico[def];
            }
            set
            {
                _dico[def] = value;
            }
        }

        public ConnectionString this[ConnectionClass clazz, ConnectionType type]
        {
            get
            {
                if (!_dico.ContainsKey(new ConnectionDefinition(clazz, type)))
                    _dico[new ConnectionDefinition(clazz, type)] = new ConnectionString();
                return this[new ConnectionDefinition(clazz, type)];
            }
            set
            {
                _dico[new ConnectionDefinition(clazz, type)] = value;
            }
        } 
    }
}
