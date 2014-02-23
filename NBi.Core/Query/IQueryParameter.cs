using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Query
{
    public interface IQueryParameter
    {
        string Name { get; set; }
        string SqlType { get; set; }
        string StringValue { get; set; }

        T GetValue<T>();
        
    }
}
