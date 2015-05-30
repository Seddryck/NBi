﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core;
using NBi.Core.Structure;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class SubsetOfConstraint : AbstractCollectionConstraint
    {
        
        /// <summary>
        /// Construct a CollectionSubsetConstraint
        /// </summary>
        /// <param name="expected"></param>
        public SubsetOfConstraint(IEnumerable<string> expected)
            : base(expected)
        {
            InternalConstraint = new CollectionSubsetConstraint(expected);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new SubsetOfConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
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
            if (Command != null)
            {
                var description = new DescriptionStructureHelper();
                var filterExpression = description.GetFilterExpression(Command.Description.Filters);
                var nextTargetExpression = description.GetNextTargetPluralExpression(Command.Description.Target);
                var expectationExpression = new StringBuilder();
                foreach (string item in Expected)
                    expectationExpression.AppendFormat("<{0}>, ", item);
                expectationExpression.Remove(expectationExpression.Length - 2, 2);

                writer.WritePredicate(string.Format("All {0} are defined in the set '{1}' for {2}",
                    nextTargetExpression,
                    expectationExpression.ToString(),
                    filterExpression));
            }
        }
    }
}
