using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Assemblies;

/// <summary>
/// This class let you convert an object into a specific type 
/// </summary>
public class TypeConverter
{
    public object Convert(object value, Type destinationType)
    {
        if (!destinationType.IsEnum)
            return System.Convert.ChangeType(value, destinationType);
        else
            return Enum.Parse(destinationType, value.ToString()!);
    }
}
