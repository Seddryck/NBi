using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet;

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


        protected string GetFunctionLabel(string function)
        {
            switch (function.ToLower())
            {
                case "children":
                    return "child";
                case "members":
                    return "member";
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
