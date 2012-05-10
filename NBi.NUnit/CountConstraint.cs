using System;
using System.Collections;
using NBi.Core;
using NBi.Core.Database;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class CountConstraint : NUnitCtr.Constraint
    {
        /// <summary>
        /// Engine dedicated to query parsing
        /// </summary>
        protected ICollectionEngine _engine;

        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected Result _res;

        /// <summary>
        /// .ctor, define the default engine used by this constraint
        /// </summary>
        public CountConstraint()
        {
            _engine = new CollectionEngine();
        }

        /// <summary>
        /// .ctor mainly used for mocking
        /// </summary>
        /// <param name="engine">The engine to use</param>
        protected internal CountConstraint(ICollectionEngine engine)
        {
            _engine = engine;
        }

        public CountConstraint Exactly(int value)
        {
            _engine.Exactly = value; 
            return this;
        }

        public CountConstraint MoreThan(int value)
        {
            _engine.MoreThan = value;
            return this;
        }

        public CountConstraint LessThan(int value)
        {
            _engine.LessThan = value; 
            return this;
        }

        public override bool Matches(object actual)
        {
            if (actual is ICollection)
                return Matches((ICollection)actual);

            return false;
        }

        /// <summary>
        /// Handle a ICollection and check it with the engine
        /// </summary>
        /// <param name="actual">an ICollection</param>
        /// <returns></returns>
        public bool Matches(ICollection actual)
        {
            _res = _engine.Validate(actual);
            return _res.ToBoolean();
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Failed!");
            foreach (var failure in _res.Failures)
            {
                sb.AppendLine(failure);
            }
            writer.WritePredicate(sb.ToString());
            //writer.WriteExpectedValue("");
        }
    }
}
