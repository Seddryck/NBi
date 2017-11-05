using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NBi.Core.ResultSet;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Core.Query.Resolver;

namespace NBi.NUnit.Member
{
    public abstract class AbstractMembersCollectionConstraint : AbstractMembersConstraint
    {

        private readonly IDbCommand commandToRetrieveExpectedItems;
        private readonly MembersDiscoveryRequest membersDiscoveryRequest;
        
        private IEnumerable<string> expectedItems;

        protected IEnumerable<string> ExpectedItems
        {
            get
            {
                return expectedItems;
            }
        }

        private IItemsMessageFormatter failure;
        protected internal IItemsMessageFormatter Failure
        {
            get
            {
                if (failure == null)
                    failure = BuildFailure();
                return failure;
            }
            set
            {
                failure = value;
            }
        }

        protected IItemsMessageFormatter BuildFailure()
        {
            var factory = new ItemsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            var compare = new ListComparer()
                        .Compare
                        (
                            ExpectedItems
                            , ((MemberResult)actual).ToCaptions()
                            , GetComparisonType()
                        );

            msg.Build(ExpectedItems, ((MemberResult)actual).ToCaptions(), compare);
            return msg;
        }

        /// <summary>
        /// Construct a AbstractMembersConstraint
        /// </summary>
        /// <param name="expected">The command to retrieve the list of expected items</param>
        public AbstractMembersCollectionConstraint(IDbCommand expected)
            : base()
        {
            commandToRetrieveExpectedItems = expected;
        }

        /// <summary>
        /// Construct a AbstractMembersConstraint
        /// </summary>
        /// <param name="expected">The list of expected items</param>
        public AbstractMembersCollectionConstraint(IEnumerable<string> expected)
            : base()
        {
            expectedItems = expected;
        }

        /// <summary>
        /// Construct a AbstractMembersConstraint
        /// </summary>
        /// <param name="expected">The request to discover members in a hierarchy or level</param>
        public AbstractMembersCollectionConstraint(MembersDiscoveryRequest expected)
            : base()
        {
            membersDiscoveryRequest = expected;
        }

       
        #region Specific NUnit

        protected override void PreInitializeMatching()
        {
            if (commandToRetrieveExpectedItems != null)
                expectedItems = GetMembersFromResultSet(commandToRetrieveExpectedItems);
            if (membersDiscoveryRequest != null)
                expectedItems = GetMembersFromDiscoveryRequest(membersDiscoveryRequest);
        }


        protected IEnumerable<string> GetMembersFromResultSet(Object obj)
        {
            if (obj is IDbCommand)
                obj = new DbCommandQueryResolverArgs((IDbCommand)obj);

            var rs = ResultSetBuilder.Build(obj);

            var members = new List<string>();
            foreach (DataRow row in rs.Rows)
                members.Add(row.ItemArray[0].ToString());

            return members;
        }

        protected IEnumerable<string> GetMembersFromDiscoveryRequest(MembersDiscoveryRequest disco)
        {
            var engine = new MembersAdomdEngine();
            var results = engine.GetMembers(disco);

            var members = results.ToCaptions();
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
                resultSetBuilder = value ?? throw new ArgumentNullException();
            }
        }


        #endregion

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        //public override void WriteDescriptionTo(MessageWriter writer)
        //{
        //    if (Request != null)
        //    {
        //        writer.WritePredicate(string.Format("On perspective \"{0}\", {1} "
        //                                                    , Request.Perspective
        //                                                    , GetPredicate()));

        //        writer.WriteLine();
        //        writer.WriteExpectedValue(Failure.RenderExpected());
        //        writer.WriteLine();
        //        writer.WriteActualValue("But was: " + Failure.RenderActual());
        //        writer.WriteLine();
        //        writer.WriteLine(Failure.RenderCompared());
        //    }
        //}

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine(Failure.RenderExpected());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine(Failure.RenderActual());
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate(string.Format("On perspective \"{0}\", {1} "
                                                            , Request.Perspective
                                                            , GetPredicate()));
            writer.WriteLine();
            writer.WriteLine();
            base.WriteMessageTo(writer);
            writer.WriteLine();
            writer.WriteLine(Failure.RenderAnalysis());
        }

        protected abstract ListComparer.Comparison GetComparisonType();
        protected abstract string GetPredicate();

       

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
