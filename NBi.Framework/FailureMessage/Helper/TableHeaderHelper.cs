﻿using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Helper
{
    public class TableHeaderHelper
    {

        public virtual string GetText(ColumnRole role, ColumnType type, Tolerance tolerance, Rounding rounding)
        {
            var roleText = GetRoleText(role);
            var typeText = GetTypeText(type);
            var toleranceText = GetToleranceText(tolerance);
            var roundingText = GetRoundingText(rounding);

            var value = string.Format("{0} ({1}){2}{3}{4}", roleText, typeText, (toleranceText + roundingText).Length > 0 ? " " : "", toleranceText, roundingText);

            return value;
        }

        private string GetRoleText(ColumnRole role)
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

        private string GetTypeText(ColumnType type)
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

        private string GetToleranceText(Tolerance tolerance)
        {
            var toleranceText = string.Empty;
            if (tolerance != null)
                toleranceText += string.Format(" (+/- {0}) ", tolerance.ValueString);
            return toleranceText;
        }

        private string GetRoundingText(Rounding rounding)
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