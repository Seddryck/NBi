using System;
using System.Linq;
using NBi.Service;

namespace NBi.GenbiL.Action.Case
{
    class FilterCaseAction : ICaseAction
    {
        public string Text { get; set; }
        public string Column { get; set; }
        public bool Negation { get; set; }
        public Operator Operator { get; set; }

        public FilterCaseAction(string column, Operator @operator, string text, bool negation)
        {
            Text = text;
            Operator = @operator;
            Column = column;
            Negation = negation;
        }
        public void Execute(GenerationState state)
        {
            state.TestCases.Filter(Column, Operator, Negation, Text);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Filtering on column '{0}' all instances {1}{2} '{3}'", Column, Negation ? "not " : string.Empty, GetOperatorText(Operator), Text);
            }
        }

        private string GetOperatorText(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    return "equal to";
                case Operator.Like:
                    return "like";
                default:
                    break;
            }
            throw new ArgumentException();
        }
    }
}
