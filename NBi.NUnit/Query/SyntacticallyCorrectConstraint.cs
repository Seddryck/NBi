using System;
using System.Data;
using NBi.Core.Query;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Query.Validation;
using NBi.Extensibility.Query;

namespace NBi.NUnit.Query
{
    public class SyntacticallyCorrectConstraint : NBiConstraint
    {
        private ParserResult parserResult;
        private IValidationEngine engine;
        protected internal IValidationEngine Engine
            { set => engine = value ?? throw new ArgumentNullException(); }

        protected IValidationEngine GetEngine(IQuery actual)
            => engine ?? (engine = new ValidationEngineFactory().Instantiate(actual));

        public SyntacticallyCorrectConstraint()
        { }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        public override bool Matches(object actual)
        {
            if (actual is IQuery)
                return doMatch((IQuery)actual);
            else
                return false;               
        }
        
        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is syntatically correct else false</returns>
        protected bool doMatch(IQuery actual)
        {
            parserResult= GetEngine(actual).Parse();
            return parserResult.IsSuccesful;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Query is not syntactically correct.");
            foreach (var failure in parserResult.Errors)
                sb.AppendLine(failure);    
            writer.WritePredicate(sb.ToString());
        }
    }
}