using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public class Rounding
    {
        public enum RoudingStyle
        {
            Undefined = 0,
            Floor,
            Ceiling,
            Round
        }

        public string Step { get; private set; }
        public RoudingStyle Style { get; private set; }

        protected Rounding(string step, RoudingStyle style)
        {
            Step = step;
            Style = style; 
        }

        protected double GetValue(double value, double step)
        {
            var remainder = Math.Abs(value) % step;

            if ((Style == RoudingStyle.Ceiling && remainder>0) || (Style == RoudingStyle.Round && remainder > step / 2))
                remainder -= step;

            return (value - remainder) * Math.Sign(value);
        }
    }
}
