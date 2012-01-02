using System;
using System.Collections.Generic;

namespace NBi.Core
{
    public class Result
    {
        protected List<string> _reasons;

        public enum ValueType
        {
            Failed,
            Success
        }

        public ValueType Value { get; protected set; }
        public string[] Reasons { get { return _reasons.ToArray(); } }

        public Result(ValueType value)
        {
            Value = value;
            _reasons = new List<String>();
        }

        public Result(ValueType value, string reason) : this(value)
        {
            _reasons.Add(reason);
        }

        public Result(ValueType value, string[] reasons)
            : this(value)
        {
            _reasons.AddRange(reasons);
        }

        public void AddReason(string reason)
        {
            _reasons.Add(reason);
        }

        public static Result Success()
        {
            var res = new Result(ValueType.Success);
            return res;
        }

        public static Result Failed()
        {
            var res = new Result(ValueType.Failed);
            return res;
        }

        public static Result Failed(string reason)
        {
            var res = new Result(ValueType.Failed, reason);
            return res;
        }

        public static Result Failed(string[] reasons)
        {
            var res = new Result(ValueType.Failed, reasons);
            return res;
        }

    }
}
