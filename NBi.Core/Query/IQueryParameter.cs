using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Query
{
    public interface IQueryParameter
    {
        string Name { get;}
        string SqlType { get; }
        object GetValue();
        
    }
}
