using NBi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case
{
    public class ReplaceCaseAction : ICaseAction
    {
        public string VariableName { get; private set; }
        public string NewValue { get; private set; }

        public string Text { get; set; }
        public string ConditionColumn { get; set; }
        public bool Negation { get; set; }
        public Operator Operator { get; set; }
        public ReplaceCaseAction(string variableName, string newValue)
        {
            VariableName = variableName;
            NewValue = newValue;
        }

        public ReplaceCaseAction(string variableName, string newValue, string conditionColumn, Operator @operator, string text, bool negation)
            : this (variableName, newValue)
        {
            ConditionColumn = conditionColumn;
            Operator = @operator;
            Text = text;
            Negation = negation;
        }

        public void Execute(GenerationState state)
        {
            foreach (DataRow row in state.TestCaseCollection.Scope.Content.Rows)
                row[VariableName] = NewValue;

            state.TestCaseCollection.Scope.Content.AcceptChanges();
                
        }

        public string Display
        {
            get 
            {
                return string.Format("Replacing values in column '{0}' by '{1}'", VariableName, NewValue);
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
