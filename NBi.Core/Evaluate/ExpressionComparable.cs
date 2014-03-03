﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Evaluate
{
    public class ExpressionComparable
    {
        private bool isParsed;

        protected string Sentence { get; private set; }
        public string Expression { get; private set; }
        public ExpressionComparer Comparer { get; private set; }

        public ExpressionComparable(string sentence)
        {
            Sentence = sentence;
        }

        public void Parse()
        {
            var sentence = Sentence.Replace(" ", "");
            var index = sentence.IndexOf("=");
            Comparer = new ExpressionComparer();
            switch (sentence.Substring(0, index + 1))
            {
                case "=": Comparer.Compare = Comparer.Equal; break;
                case "!=": Comparer.Compare = Comparer.NotEqual; break;
                //case ">": Compare = Greater; break;
                //case ">=": Compare = GreaterOrEqual; break;
                //case "<": Compare = Smaller; break;
                //case "<=": Compare = SmallerOrEqual; break;
                default:
                    {
                        Comparer = null;
                        throw new InvalidExpressionException(string.Format("An expression must start by '=' or '!=' but the expression '{0}' is not.", Sentence));
                    }
                    
            }
            Expression = sentence.Substring(index + 1);
            isParsed = true;
        }

        public Object Evaluate(Dictionary<string, Object> variables)
        {
            if (!isParsed)
                throw new InvalidOperationException("Before calling the Evaluate function, you must call the Parse function.");

            var exp = new NCalc.Expression(Expression);
            exp.Parameters = variables;
            return exp.Evaluate();
        }
    }
}
