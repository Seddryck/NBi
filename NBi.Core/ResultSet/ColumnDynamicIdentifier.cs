using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

//class ColumnDynamicIdentifier : ColumnOrdinalIdentifier, IEquatable<ColumnDynamicIdentifier>
//{
//    public string Name { get; }
//    private Func<int, int> InternalNext { get; }

//    public override string Label => $"&{Name}";

//    public ColumnDynamicIdentifier(string name, Func<int, int> nextFunction)
//        : base(0)
//    {
//        Name = name;
//        InternalNext = nextFunction;
//    }

//    public bool Next()
//    {
//        Ordinal = InternalNext(Ordinal);
//        return true;
//    }
    

//    public override bool Equals(object obj) => this.Equals(obj as ColumnDynamicIdentifier);
//    public override int GetHashCode() => Name.GetHashCode() ^35 * Ordinal.GetHashCode() ^17;

//    public bool Equals(ColumnDynamicIdentifier other)
//    {
//        if (other is null)
//            return false;
//        return (other.Name == Name && other.Ordinal == Ordinal);
//    }
//}
