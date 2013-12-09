using System;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Query
{
    public class MatchPatternConstraint : NUnitCtr.Constraint
    {
        private string regex;
        private readonly FormattedResults invalidMembers = new FormattedResults();
        protected IQueryFormat engine;
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected internal IQueryFormat Engine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                engine = value;
            }
        }

        protected IQueryFormat GetEngine(IDbCommand actual)
        {
            if (engine == null)
                engine = new QueryEngineFactory().GetFormat(actual);
            return engine;
        }

        #region Modifiers
        /// <summary>
        /// Set the regex pattern
        /// </summary>
        public MatchPatternConstraint Regex(string pattern)
        {
            this.regex = pattern;
            return this;
        }
        #endregion

        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected FormattedResults formattedResults;

        protected virtual NUnitCtr.Constraint BuildInternalConstraint()
        {
            NUnitCtr.Constraint ctr = null;

            if (!string.IsNullOrEmpty(regex))
            {
                if (ctr != null)
                    ctr = ctr.And.Matches(regex);
                else
                    ctr = new NUnitCtr.RegexConstraint(regex);
            }

            return ctr;
        }

        protected virtual bool DoMatch(NUnitCtr.Constraint ctr, string caption)
        {
            IResolveConstraint exp = ctr;
            var multipleConstraint = exp.Resolve();
            return multipleConstraint.Matches(caption);
        }

        public override bool Matches(object actual)
        {
            if (actual is IDbCommand)
                return Process((IDbCommand)actual);
            else if (actual is FormattedResults)
            {
                this.actual = actual;

                var res = true;
                foreach (var result in (FormattedResults)actual)
                {
                    var ctr = BuildInternalConstraint();
                    if (!DoMatch(ctr, result))
                    {
                        res = false;
                        invalidMembers.Add(result);
                    }
                }
                return res;
            }
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IDbCommand actual)
        {
            FormattedResults result = GetEngine(actual).GetFormats();

            return this.Matches(result);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            //writer.WriteActualValue(actual);

            writer.WritePredicate(string.Format("The formatted value of each cell matchs the"));

            if (!string.IsNullOrEmpty(regex))
            {
                writer.WritePredicate(" regex pattern ");
                writer.WritePredicate(regex);
            }
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (invalidMembers.Count == 1)
                writer.WriteMessageLine(string.Format("The element <{0}> doesn't validate this pattern", invalidMembers[0]));
            else
            {
                writer.WriteMessageLine(string.Format("{0} elements don't validate this pattern:", invalidMembers.Count));
                foreach (var invalidMember in invalidMembers)
                    writer.WriteMessageLine(string.Format("    <{0}>", invalidMember));
            }
        }
    }
}
