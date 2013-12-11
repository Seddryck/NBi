using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges
{
    public class RangeMembersFactory
    {
        private readonly ICollection<BuilderRegistration> registrations;

        public RangeMembersFactory()
        {
            registrations = new List<BuilderRegistration>();
            RegisterDefaults();
        }

        private void RegisterDefaults()
        {
            Register(typeof(IntegerRange), new IntegerRangeBuilder());
            Register(typeof(DateRange), new DateRangeBuilder());
        }

        /// <summary>
        /// Register a new builder for corresponding types. If a builder was already existing for this association, it's replaced by the new one
        /// </summary>
        /// <param name="sutType">Type of System Under Test</param>
        /// <param name="ctrType">Type of Constraint</param>
        /// <param name="builder">Instance of builder deicated for these types of System Under Test and Constraint</param>
        public void Register(Type rangeType, IRangeMembersBuilder builder)
        {
            if (IsHandling(rangeType))
                registrations.FirstOrDefault(reg => reg.Type == rangeType).Builder = builder;
            else
                registrations.Add(new BuilderRegistration(rangeType, builder));
        }

        private bool IsHandling(Type rangeType)
        {
            var existing = registrations.FirstOrDefault(reg => reg.Type == rangeType);
            return (existing != null);
        }

        private class BuilderRegistration
        {
            public Type Type { get; set; }
            public IRangeMembersBuilder Builder { get; set; }

            public BuilderRegistration(Type rangeType, IRangeMembersBuilder builder)
            {
                Type = rangeType;
                Builder = builder;
            }
        }

        /// <summary>
        /// Create a new instance of a test case
        /// </summary>
        /// <param name="sutXml"></param>
        /// <param name="ctrXml"></param>
        /// <returns></returns>
        public IEnumerable<string> Instantiate(IRange range)
        {

            if (range == null)
                throw new ArgumentNullException("range");

            IRangeMembersBuilder builder = null;

            //Look for registration ...
            var registration = registrations.FirstOrDefault(reg => reg.Type == range.GetType());
            if (registration == null)
                throw new ArgumentException(string.Format("'{0}' has no builder registred.", range.GetType().Name, "range"));

            //Get Builder and initiate it
            builder = registration.Builder;
            builder.Setup(range);

            //Build
            builder.Build();
            var list = builder.GetResult();

            return list;
        }
    }
}
