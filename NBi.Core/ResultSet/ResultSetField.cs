using System;

namespace NBi.Core.ResultSet
{
    public abstract class ResultSetField
    {
        public Object Raw { get; private set; }

        public ResultSetField(string raw)
        {
            Raw = raw;
        }
    }

    public class ResultSetKey : ResultSetField
    {
        public bool EqualTo(ResultSetKey other)
        {
            return this.Value == other.Value;
        }

        public new string Value
        {
            get
            {
                return (string)Raw;
            }
        }
        
        public ResultSetKey(string raw) : base(raw) {}


    }

    public class ResultSetValue : ResultSetField
    {
        public new decimal Value { get { return decimal.Parse(Raw.ToString(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat); } }

        public ResultSetValue(string raw) : base(raw) { }

        public bool EqualTo(ResultSetValue other, decimal tolerance)
        {
            return (Math.Abs(this.Value - other.Value)<=tolerance);
        }
    }
}
