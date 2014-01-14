using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ContainConstraint : AbstractCollectionConstraint
    {
        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(string expected)
            : this(new List<string>() {expected})
        {
        }
        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(IEnumerable<string> expected)
            : base(expected)
        {
            BuildInternalConstraint();
        }

        /// <summary>
        /// Build a collection of constraint (in fact one by expected item)
        /// Pay really attention that CollectionContainsConstraint is expecting one unique item!
        /// So if you cant to check for several items, you need to apply several Constraint (one by expected item)
        /// </summary>
        protected virtual void BuildInternalConstraint()
        {
            NUnitCtr.Constraint ctr = null;
            foreach (var item in Expected)
            {
                var localCtr = new NUnitCtr.CollectionContainsConstraint(StringComparerHelper.Build(item));
                var usingCtr = localCtr.Using(Comparer);

                if (ctr != null)
                    ctr = new AndConstraint(ctr, usingCtr);
                else
                    ctr = usingCtr;
            }

            if (ctr == null)
                ctr = new NUnitCtr.EmptyCollectionConstraint();

            IResolveConstraint exp = ctr;
            InternalConstraint = exp.Resolve();
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// Pay attention that we need to rebuild the constraint from scratch 
        /// because the Using is applied for each constraint contained in this constraint
        /// </summary>
        public new ContainConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
                BuildInternalConstraint();
                return this;
            }
        }

        #endregion

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            var description = new DescriptionStructureHelper();
            var filterExpression = description.GetFilterExpression(Request.GetAllFilters());

            if (Expected.Count() == 1)
            {
                writer.WritePredicate(string.Format("find a {0} named '{1}' contained {2}",
                    description.GetTargetExpression(Request.Target),
                    Expected.First(),
                    filterExpression));
            }
            else
            {
                var expectationExpression = new StringBuilder();
                foreach (string item in Expected)
                    expectationExpression.AppendFormat("<{0}>, ", item);
                expectationExpression.Remove(expectationExpression.Length - 2, 2);

                writer.WritePredicate(string.Format("find the {0} named '{1}' contained {2}",
                    description.GetTargetPluralExpression(Request.Target),
                    expectationExpression.ToString(),
                    filterExpression));
            }
        }
    }
}
