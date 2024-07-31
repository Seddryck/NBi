using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown.Helper
{
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

        public string GetRoleText(ColumnRole role)
        {
            switch (role)
            {
                case ColumnRole.Key:
                    return "KEY";
                case ColumnRole.Value:
                    return "VALUE"; 
                default:
                    return string.Empty;
            }
        }

        public string GetTypeText(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Numeric:
                    return "Numeric";
                case ColumnType.Text:
                    return "Text";
                case ColumnType.DateTime:
                    return "DateTime";
                case ColumnType.Boolean:
                    return "Boolean";
            }
            return "?";
        }

        public string GetToleranceText(Tolerance? tolerance)
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

        private string GetRoundingStyleText(Rounding rounding)
        {
            switch (rounding.Style)
            {
                case Rounding.RoundingStyle.None:
                    return string.Empty;
                case Rounding.RoundingStyle.Floor:
                    return "floor";
                case Rounding.RoundingStyle.Round:
                    return "round";
                case Rounding.RoundingStyle.Ceiling:
                    return "ceiling";
            }
            return "?";
        }

    }
}