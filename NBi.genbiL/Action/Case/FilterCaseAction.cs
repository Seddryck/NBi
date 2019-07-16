using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using System.Collections.Generic;

namespace NBi.GenbiL.Action.Case
{
    class FilterCaseAction : ICaseAction
    {
        public IEnumerable<string> Values { get; set; }
        public string Column { get; set; }
        public bool Negation { get; set; }
        public OperatorType Operator { get; set; }

        public FilterCaseAction(string column, OperatorType @operator, IEnumerable<string> values, bool negation)
        {
            Values = values;
            Operator = @operator;
            Column = column;
            Negation = negation;
        }
        public void Execute(GenerationState state)
        {
            state.TestCaseCollection.Scope.Filter(Column, Operator, Negation, Values);
        }

        public virtual string Display
        {
            get
            {
                return string.Format(
                    "Filtering on column '{0}' all instances {1}{2} '{3}'"
                    , Column
                    , Negation ? "not " : string.Empty
                    , GetOperatorText(Operator)
                    , string.Join("', '", Values));
            }
        }

        private string GetOperatorText(OperatorType @operator)
        {
            switch (@operator)
            {
                case OperatorType.Equal:
                    return "equal to";
                case OperatorType.Like:
                    return "like";
                default:
                    break;
            }
            throw new ArgumentException();
        }
    }
}
