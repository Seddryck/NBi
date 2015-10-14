using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class KeyCollection : IEquatable<KeyCollection>
    {
        public object[] Members { get; private set; }

        public KeyCollection(object[] members)
        {
            Members = members;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 0;
                for (int i = 0; i < this.Members.Length; i++)
                {
                    hash = hash ^ 397 * Members[i].GetHashCode();
                }
                return hash;
            }
        }

        public bool Equals(KeyCollection other)
        {
            if (other == null && this == null)
                return true;
            if (other == null || this == null)
                return false;
            if (other.Members == null && this.Members == null)
                return true;
            if (other.Members == null || this.Members == null)
                return false;

            if (other.Members.Length != this.Members.Length)
                return false;

            for (int i = 0; i < this.Members.Length; i++)
            {
                if (!other.Members[i].Equals(this.Members[i]))
                    return false;
            }
            return true;
    
        }
    }
}
