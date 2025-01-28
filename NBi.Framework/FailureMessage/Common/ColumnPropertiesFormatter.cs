using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown.Helper;

public class ColumnPropertiesFormatter
{

    public virtual string GetText(IColumnDefinition definition)
        => GetText(definition.Role, definition.Type, new ToleranceFactory().Instantiate(definition) , null);

    public virtual string GetText(ColumnMetadata metadata)
        => GetText(metadata.Role, metadata.Type, metadata.Tolerance, metadata.Rounding);

    public virtual string GetText(ColumnRole role, ColumnType type, Tolerance? tolerance, Rounding? rounding)
    {
        var roleText = GetRoleText(role);
        if (string.IsNullOrEmpty(roleText))
            return string.Empty;

        var typeText = GetTypeText(type);
        var toleranceText = GetToleranceText(tolerance);
        var roundingText = GetRoundingText(rounding);

        var value = string.Format("{0} ({1}){2}{3}{4}", roleText, typeText, (toleranceText + roundingText).Length > 0 ? " " : "", toleranceText, roundingText);

        return value;
    }

    public virtual string GetRoleText(ColumnRole role)
    {
        return role switch
        {
            ColumnRole.Key => "KEY",
            ColumnRole.Value => "VALUE",
            _ => string.Empty,
        };
    }

    public virtual string GetTypeText(ColumnType type)
    {
        return type switch
        {
            ColumnType.Numeric => "Numeric",
            ColumnType.Text => "Text",
            ColumnType.DateTime => "DateTime",
            ColumnType.Boolean => "Boolean",
            _ => "?",
        };
    }

    public virtual string GetToleranceText(Tolerance? tolerance)
    {
        var toleranceText = string.Empty;
        if (tolerance != null && tolerance!=TextSingleMethodTolerance.None && tolerance!=DateTimeTolerance.None && tolerance!=NumericAbsoluteTolerance.None)
            toleranceText += string.Format(" (+/- {0}) ", tolerance.ValueString);
        return toleranceText;
    }

    public string GetRoundingText(Rounding? rounding)
    {
        var roundingText = string.Empty;
        if (rounding != null)
            roundingText += string.Format(" ({0} {1}) ", GetRoundingStyleText(rounding), rounding.Step);
        return roundingText;
    }

    protected virtual string GetRoundingStyleText(Rounding rounding)
    {
        return rounding.Style switch
        {
            Rounding.RoundingStyle.None => string.Empty,
            Rounding.RoundingStyle.Floor => "floor",
            Rounding.RoundingStyle.Round => "round",
            Rounding.RoundingStyle.Ceiling => "ceiling",
            _ => "?",
        };
    }

}