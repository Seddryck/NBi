using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core;

public static class AttributeExtensions
{
    public static TValue? GetAttributeValue<TAttribute, TValue>(
        this Type type,
        Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
    {
        if (type.GetCustomAttributes(
            typeof(TAttribute), true
        ).FirstOrDefault() is TAttribute att)
        {
            return valueSelector(att);
        }
        return default;
    }
}
