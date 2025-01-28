using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting;

class BooleanCaster : BaseNumericCaster, ICaster<bool>
{
    public bool Execute(object? value)
    {
        if (value is null)
            throw new ArgumentOutOfRangeException(nameof(value));

        if (value is bool v)
            return v;

        var boolValue = IntParsing(value);
        if (boolValue != ThreeStateBoolean.Unknown)
            return boolValue == ThreeStateBoolean.True;

        boolValue = StringParsing(value);
        if (boolValue != ThreeStateBoolean.Unknown)
            return boolValue == ThreeStateBoolean.True;
        throw new ArgumentOutOfRangeException(nameof(value));
    }

    object ICaster.Execute(object? value) => Execute(value);


    public override bool IsValid(object? value)
        => value switch
        {
            null => false,
            bool => true,
            string str => StringParsing(value) != ThreeStateBoolean.Unknown,
            _ => base.IsValid(value)
        };

    protected ThreeStateBoolean IntParsing(object obj)
    =>  (IsParsableNumeric(obj) 
                            ? Convert.ToDecimal(obj, NumberFormatInfo.InvariantInfo) 
                            : decimal.MinValue
        ) switch
            {
                decimal.Zero => ThreeStateBoolean.False,
                decimal.One => ThreeStateBoolean.True,
                _ => ThreeStateBoolean.Unknown
            };

    protected virtual ThreeStateBoolean StringParsing(object obj)
        => (obj.ToString() ?? string.Empty).ToLowerInvariant() switch
        {
            "false" => ThreeStateBoolean.False,
            "no" => ThreeStateBoolean.False,
            "true" => ThreeStateBoolean.True,
            "yes" => ThreeStateBoolean.True,
            _ => ThreeStateBoolean.Unknown
        };

    protected virtual string ThreeStateToString(ThreeStateBoolean ts, string defaultValue)
        => ts switch
        {
            ThreeStateBoolean.False => "false",
            ThreeStateBoolean.True => "true",
            ThreeStateBoolean.Unknown => defaultValue,
            _ => defaultValue
        };
}
