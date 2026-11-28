using System.ComponentModel;
using System;
using System.Linq;

namespace NBi.Model.Items;

public abstract class Parameter
{
    public bool IsRemoved { get; set; } = false;

    public string Name { get; set; } = string.Empty;

    public string StringValue { get; set; } = string.Empty;

    public virtual T GetValue<T>()
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter != null)
        {
            var stringWithoutSpecialChars = StringValue.Replace("\n", "").Replace("\t", "").Replace("\n", "").Trim();
            return (T)(converter.ConvertFrom(stringWithoutSpecialChars) ?? throw new NullReferenceException());
        }
        else
            throw new InvalidOperationException();
    }
}
