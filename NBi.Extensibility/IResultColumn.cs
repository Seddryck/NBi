using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility
{
    public interface IResultColumn
    {
        string Name { get; }    
        Type DataType { get; }
        int Ordinal { get; }

        void Rename(string newName);
        void Move(int ordinal);
        void Remove();
        void ReplaceBy(IResultColumn column);

        void SetProperties(object role, object type, object? tolerance, object? rounding);
        void SetProperties(object role, object type);
        bool HasProperties();
        object? GetProperty(string property);
    }
}
