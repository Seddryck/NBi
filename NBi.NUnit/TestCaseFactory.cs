using System;
using System.Collections.Generic;
using NBi.NUnit.Builder;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using System.Linq;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;
using NBi.Core.Variable;
using NBi.Core.Injection;
using NBi.Core.Configuration;

namespace NBi.NUnit
{
    public class TestCaseFactory
    {
        private readonly ICollection<BuilderRegistration> registrations;
        private readonly IConfiguration configuration;
        private readonly IDictionary<string, ITestVariable> variables;
        private readonly ServiceLocator serviceLocator;

        public TestCaseFactory()
            : this(Configuration.Default, new Dictionary<string, ITestVariable>(), null)
        {
        }

        public TestCaseFactory(IConfiguration configuration, IDictionary<string, ITestVariable> variables, ServiceLocator serviceLocator)
        {
            this.configuration = configuration;
            this.variables = variables;
            this.serviceLocator = serviceLocator;
            registrations = new List<BuilderRegistration>();
            RegisterDefault();
        }

        /// <summary>
        /// Register the values for production usage
        /// </summary>
        private void RegisterDefault()
        {
            Register(typeof(ExecutionXml), typeof(FasterThanXml), new ExecutionFasterThanChooser());
            Register(typeof(ExecutionXml), typeof(SyntacticallyCorrectXml), new ExecutionSyntacticallyCorrectBuilder());
            Register(typeof(ExecutionXml), typeof(SuccessfulXml), new ExecutionNonQuerySuccessfulBuilder());

            Register(typeof(ExecutionXml), typeof(MatchPatternXml), new ExecutionMatchPatternBuilder());
            Register(typeof(ExecutionXml), typeof(EvaluateRowsXml), new ExecutionEvaluateRowsBuilder());

            Register(typeof(ExecutionXml), typeof(EqualToXml), new ResultSetEqualToBuilder());
            Register(typeof(ExecutionXml), typeof(SupersetOfXml), new ResultSetSupersetOfBuilder());
            Register(typeof(ExecutionXml), typeof(SubsetOfXml), new ResultSetSubsetOfBuilder());
            Register(typeof(ExecutionXml), typeof(RowCountXml), new ResultSetRowCountBuilder());
            Register(typeof(ExecutionXml), typeof(AllRowsXml), new ResultSetAllRowsBuilder());
            Register(typeof(ExecutionXml), typeof(NoRowsXml), new ResultSetNoRowsBuilder());
            Register(typeof(ExecutionXml), typeof(SomeRowsXml), new ResultSetSomeRowsBuilder());
            Register(typeof(ExecutionXml), typeof(SingleRowXml), new ResultSetSingleRowBuilder());
            Register(typeof(ExecutionXml), typeof(UniqueRowsXml), new ResultSetUniqueRowsBuilder());

            Register(typeof(ResultSetSystemXml), typeof(EqualToXml), new ResultSetEqualToBuilder());
            Register(typeof(ResultSetSystemXml), typeof(SupersetOfXml), new ResultSetSupersetOfBuilder());
            Register(typeof(ResultSetSystemXml), typeof(SubsetOfXml), new ResultSetSubsetOfBuilder());
            Register(typeof(ResultSetSystemXml), typeof(RowCountXml), new ResultSetRowCountBuilder());
            Register(typeof(ResultSetSystemXml), typeof(AllRowsXml), new ResultSetAllRowsBuilder());
            Register(typeof(ResultSetSystemXml), typeof(NoRowsXml), new ResultSetNoRowsBuilder());
            Register(typeof(ResultSetSystemXml), typeof(SomeRowsXml), new ResultSetSomeRowsBuilder());
            Register(typeof(ResultSetSystemXml), typeof(SingleRowXml), new ResultSetSingleRowBuilder());
            Register(typeof(ResultSetSystemXml), typeof(UniqueRowsXml), new ResultSetUniqueRowsBuilder());
            Register(typeof(ResultSetSystemXml), typeof(LookupExistsXml), new ResultSetLookupExistsBuilder());

            Register(typeof(ScalarXml), typeof(ScoreXml), new ScalarScoreBuilder());

            Register(typeof(MembersXml), typeof(CountXml), new MembersCountBuilder());
            Register(typeof(MembersXml), typeof(OrderedXml), new MembersOrderedBuilder());
            Register(typeof(MembersXml), typeof(ContainXml), new MembersContainBuilder());
            Register(typeof(MembersXml), typeof(ContainedInXml), new MembersContainedInBuilder());
            Register(typeof(MembersXml), typeof(SubsetOfOldXml), new MembersContainedInBuilder());
            Register(typeof(MembersXml), typeof(EquivalentToXml), new MembersEquivalentToBuilder());
            Register(typeof(MembersXml), typeof(MatchPatternXml), new MembersMatchPatternBuilder());

            Register(typeof(StructureXml), typeof(ContainXml), new StructureContainBuilder());
            Register(typeof(StructureXml), typeof(ContainedInXml), new StructureContainedInBuilder());
            Register(typeof(StructureXml), typeof(SubsetOfOldXml), new StructureContainedInBuilder());
            Register(typeof(StructureXml), typeof(EquivalentToXml), new StructureEquivalentToBuilder());
            Register(typeof(StructureXml), typeof(ExistsXml), new StructureExistsBuilder());
            Register(typeof(StructureXml), typeof(LinkedToXml), new StructureLinkedToBuilder());

            Register(typeof(DataTypeXml), typeof(IsXml), new DataTypeIsBuilder());
        }

        /// <summary>
        /// Register a new builder for corresponding types. If a builder was already existing for this association, it's replaced by the new one
        /// </summary>
        /// <param name="sutType">Type of System Under Test</param>
        /// <param name="ctrType">Type of Constraint</param>
        /// <param name="builder">Instance of builder deicated for these types of System Under Test and Constraint</param>
        internal void Register(Type sutType, Type ctrType, ITestCaseBuilder builder)
        {
            if (IsHandling(sutType, ctrType))
                registrations.FirstOrDefault(reg => reg.SystemUnderTestType.Equals(sutType) && reg.ConstraintType.Equals(ctrType)).Builder = builder;
            else
                registrations.Add(new BuilderRegistration(sutType, ctrType, builder));
        }

        internal void Register(Type sutType, Type ctrType, ITestCaseBuilderChooser chooser)
        {
            if (IsHandling(sutType, ctrType))
                registrations.FirstOrDefault(reg => reg.SystemUnderTestType.Equals(sutType) && reg.ConstraintType.Equals(ctrType)).Chooser = chooser;
            else
                registrations.Add(new BuilderRegistration(sutType, ctrType, chooser));
        }

        internal bool IsHandling(Type sutType, Type ctrType)
        {
            var existing = registrations.FirstOrDefault(reg => reg.SystemUnderTestType.Equals(sutType) && reg.ConstraintType.Equals(ctrType));
            return (existing != null);
        }

        internal class BuilderRegistration
        {
            public Type SystemUnderTestType { get; set; }
            public Type ConstraintType { get; set; }
            public ITestCaseBuilder Builder { get; set; }
            public ITestCaseBuilderChooser Chooser { get; set; }

            private BuilderRegistration(Type sutType, Type ctrType)
            {
                SystemUnderTestType = sutType;
                ConstraintType = ctrType;
            }

            public BuilderRegistration(Type sutType, Type ctrType, ITestCaseBuilder builder)
                : this(sutType, ctrType)
            {
                Builder = builder;
            }

            public BuilderRegistration(Type sutType, Type ctrType, ITestCaseBuilderChooser chooser)
                : this(sutType, ctrType)
            {
                Chooser = chooser;
                Chooser.Target = this;
            }
        }

        /// <summary>
        /// Create a new instance of a test case
        /// </summary>
        /// <param name="sutXml"></param>
        /// <param name="ctrXml"></param>
        /// <returns></returns>
        public TestCase Instantiate(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (sutXml == null)
                throw new ArgumentNullException("sutXml");
            if (ctrXml == null)
                throw new ArgumentNullException("ctrXml");

            ITestCaseBuilder builder = null;

            //Look for registration ...
            var registration = registrations.FirstOrDefault(reg => sutXml.GetType() == reg.SystemUnderTestType && ctrXml.GetType() == reg.ConstraintType);
            if (registration == null)
                throw new ArgumentException(string.Format("'{0}' is not an expected type for a constraint with a system-under-test '{1}'.", ctrXml.GetType().Name, sutXml.GetType().Name));

            //Apply the chooser if needed
            if (registration.Builder == null)
                registration.Chooser.Choose(sutXml, ctrXml);

            //Get Builder and initiate it
            builder = registration.Builder;
            builder.Setup(sutXml, ctrXml, configuration, variables, serviceLocator);

            //Build
            builder.Build();
            NUnitCtr.Constraint ctr = builder.GetConstraint();
            var sut = builder.GetSystemUnderTest();

            //Apply negation if needed
            if (ctrXml.Not)
                ctr = new NUnitCtr.NotConstraint(ctr);

            return new TestCase(sut, ctr);
        }
    }
}
