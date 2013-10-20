using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NBi.Core.ResultSet;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public abstract class AbstractMembersCollectionConstraint : AbstractMembersConstraint
    {

        private readonly IDbCommand commandToRetrieveExpectedItems;
        private IEnumerable<string> expectedItems;

        protected IEnumerable<string> ExpectedItems
        {
            get
            {
                return expectedItems;
            }
        }
        

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected">The command to retrieve the list of expected items</param>
        public AbstractMembersCollectionConstraint(IDbCommand expected)
            : base()
        {
            commandToRetrieveExpectedItems = expected;
        }

        /// <summary>
        /// Construct a CollectionEquivalentConstraint
        /// </summary>
        /// <param name="expected">The list of expected items</param>
        public AbstractMembersCollectionConstraint(IEnumerable<string> expected)
            : base()
        {
            expectedItems = expected;
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        protected void IgnoreCase()
        {
            Comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(false);
        }

        #endregion
        
        #region Specific NUnit

        protected override void PreInitializeMatching()
        {
            if (commandToRetrieveExpectedItems != null)
                expectedItems = GetMembersFromResultSet(commandToRetrieveExpectedItems);
        }


        protected IEnumerable<string> GetMembersFromResultSet(Object obj)
        {
            var rs = ResultSetBuilder.Build(obj);

            var members = new List<string>();
            foreach (DataRow row in rs.Rows)
                members.Add(row.ItemArray[0].ToString());

            return members;
        }

        /// <summary>
        /// Engine dedicated to ResultSet acquisition
        /// </summary>
        protected IResultSetBuilder resultSetBuilder;
        protected internal IResultSetBuilder ResultSetBuilder
        {
            get
            {
                if (resultSetBuilder == null)
                    resultSetBuilder = new ResultSetBuilder();
                return resultSetBuilder;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                resultSetBuilder = value;
            }
        }


        #endregion

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Request != null)
            {
                writer.WritePredicate(string.Format("On perspective \"{0}\", {1} "
                                                            , Request.Perspective
                                                            , GetPredicate()));
                writer.WriteExpectedValue(ExpectedItems);
                
                var info = new ListComparisonFormatter()
                    .Format
                    (
                        new ListComparer()
                            .Compare
                            (
                                ((MemberResult)actual).ToCaptions()
                                , ExpectedItems
                                , GetComparisonType()
                            ).Sample()
                    );

                writer.WriteLine(info.ToString());
            }
        }

        protected abstract ListComparer.Comparison GetComparisonType();
        protected abstract string GetPredicate();

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (actual is MemberResult && ((MemberResult)actual).Count() > 0 && ((MemberResult)actual).Count() <= 15)
                writer.WriteActualValue((IEnumerable)actual);
            else if (actual is MemberResult && ((MemberResult)actual).Count() > 0 && ((MemberResult)actual).Count() > 15)
            {
                writer.WriteActualValue(((IEnumerable<NBi.Core.Analysis.Member.Member>)actual).Take(10));
                writer.WriteActualValue(string.Format(" ... and {0} others.", ((MemberResult)actual).Count() - 10));
            }
            else
                writer.WriteActualValue(new NothingFoundMessage());
        }

        protected string GetFunctionLabel(string function)
        {
            switch (function.ToLower())
            {
                case "children":
                    return "children";
                case "members":
                    return "members";
                default:
                    return "?";
            }
        }

        protected internal class NothingFoundMessage
        {
            public override string ToString()
            {
                return "nothing found";
            }
        }
    }
}
