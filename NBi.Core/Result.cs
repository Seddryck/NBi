using System;
using System.Collections.Generic;

namespace NBi.Core
{
    public class Result
    {
        protected List<string> _failures;

        public enum ValueType
        {
            Failed,
            Success
        }

        public ValueType Value { get; protected set; }
        public string[] Failures { get { return _failures.ToArray(); } }

        public Result(ValueType value)
        {
            Value = value;
            _failures = new List<String>();
        }

        public Result(ValueType value, string reason) : this(value)
        {
            _failures.Add(reason);
        }

        public Result(ValueType value, string[] failures)
            : this(value)
        {
            _failures.AddRange(failures);
        }

        public void AddFailure(string failure)
        {
            _failures.Add(failure);
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

        public static Result Failed(string failure)
        {
            var res = new Result(ValueType.Failed, failure);
            return res;
        }

        public static Result Failed(string[] failures)
        {
            var res = new Result(ValueType.Failed, failures);
            return res;
        }

    }
}
