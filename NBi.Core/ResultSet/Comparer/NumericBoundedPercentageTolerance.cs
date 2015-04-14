﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public class NumericBoundedPercentageTolerance : NumericTolerance
    {
        public decimal Min { get; private set; }
        public decimal Max { get; private set; }

        public override string ValueString
        {
            get
            {
                return string.Format("{0}% ({1}: {2})"
                    , (100 * Value).ToString(NumberFormatInfo.InvariantInfo)
                    , Min > 0 ? "min" : "max"
                    , (Min > 0 ? Min : Max).ToString(NumberFormatInfo.InvariantInfo));
            }
        }

        public NumericBoundedPercentageTolerance(decimal percentage, decimal minValue, decimal maxValue)
            : base(percentage)
        {
            if (minValue == 0 && maxValue == 0)
                throw new ArgumentException();
            if (minValue < 0 || maxValue < 0)
                throw new ArgumentException();
            
            Value = percentage;
            Min = minValue;
            Max = maxValue;
        }

        public decimal GetValue(decimal expected)
        {
            if (Min > 0)
                return Math.Max(Math.Abs(expected * Value), Min);
            else
                return Math.Min(Math.Abs(expected * Value), Max);
        }
    }
}
