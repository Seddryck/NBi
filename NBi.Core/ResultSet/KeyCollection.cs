using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

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
                hash = hash ^ 397 * GetGenericValue(Members[i]).GetHashCode();
            }
            return hash;
        }
    }

    private object GetGenericValue(object obj)
    {
        if (obj == DBNull.Value || obj == null)
            return "(null)";
        if (obj is string && string.IsNullOrEmpty(obj as string))
            return "(empty)";
        return obj;
    }

    public bool Equals(KeyCollection? other)
    {
        if (other == null && this == null)
            return true;
        if (other == null || this == null)
            return false;
        if (other.Members == null && Members == null)
            return true;
        if (other.Members == null || Members == null)
            return false;

        if (other.Members.Length != Members.Length)
            return false;

        for (int i = 0; i < Members.Length; i++)
        {
            if (!GetGenericValue(other.Members[i]).Equals(GetGenericValue(Members[i])))
                return false;
        }
        return true;

    }
}
