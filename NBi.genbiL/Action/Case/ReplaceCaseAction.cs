using System;
using System.Linq;
using System.Collections.Generic;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class ReplaceCaseAction : ICaseAction
    {
        public string Column { get; set; }
        public string NewValue { get; set; }
        public IEnumerable<string> Values { get; set; }
        public bool Negation { get; set; }
        public OperatorType Operator { get; set; }

        public ReplaceCaseAction(string column, string newValue)
        {
            Column = column;
            NewValue = newValue;
        }

        public ReplaceCaseAction(string column, string newValue, OperatorType @operator, IEnumerable<string> values, bool negation)
            : this(column, newValue)
        {
            Values = values;
            Operator = @operator;
            Negation = negation;
        }

        public void Execute(GenerationState state)
        {
            if (Values==null || Values.Count()==0)
                state.TestCaseCollection.Scope.Replace(Column, NewValue);
            else
                state.TestCaseCollection.Scope.Replace(Column, NewValue, Operator, Negation, Values);
        }

        public virtual string Display
        {
            get
            {
                var display = string.Format(
                        "Replacing content of column '{0}' with value '{1}'"
                        , Column
                        , NewValue);

                if (Values != null && Values.Count() > 0)
                    display += string.Format(
                        " when values {0}{1} '{2}'"
                        , Negation ? "not " : string.Empty
                        , GetOperatorText(Operator)
                        , string.Join("', '", Values));

                return display;
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
