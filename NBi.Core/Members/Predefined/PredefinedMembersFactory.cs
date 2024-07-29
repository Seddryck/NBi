using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Predefined
{
    public class PredefinedMembersFactory
    {
        private readonly ICollection<BuilderRegistration> registrations;

        public PredefinedMembersFactory()
        {
            registrations = new List<BuilderRegistration>();
            RegisterDefaults();
        }

        private void RegisterDefaults()
        {
            Register(PredefinedMembers.DaysOfWeek, new DaysOfWeekBuilder());
            Register(PredefinedMembers.MonthsOfYear, new MonthsOfYearBuilder());
        }

        /// <summary>
        /// Register a new builder for corresponding types. If a builder was already existing for this association, it's replaced by the new one
        /// </summary>
        /// <param name="sutType">Type of System Under Test</param>
        /// <param name="ctrType">Type of Constraint</param>
        /// <param name="builder">Instance of builder deicated for these types of System Under Test and Constraint</param>
        public void Register(PredefinedMembers value, IPredefinedMembersBuilder builder)
        {
            if (IsHandling(value))
                registrations.FirstOrDefault(reg => reg.Value == value).Builder = builder;
            else
                registrations.Add(new BuilderRegistration(value, builder));
        }

        private bool IsHandling(PredefinedMembers value)
        {
            var existing = registrations.FirstOrDefault(reg => reg.Value == value);
            return (existing != null);
        }

        private class BuilderRegistration
        {
            public PredefinedMembers Value { get; set; }
            public IPredefinedMembersBuilder Builder { get; set; }

            public BuilderRegistration(PredefinedMembers value, IPredefinedMembersBuilder builder)
            {
                Value = value;
                Builder = builder;
            }
        }

        /// <summary>
        /// Create a new instance of a test case
        /// </summary>
        /// <param name="sutXml"></param>
        /// <param name="ctrXml"></param>
        /// <returns></returns>
        public IEnumerable<string> Instantiate(PredefinedMembers value, string cultureName)
        {
            
            if (!Enum.IsDefined(typeof(PredefinedMembers), value))
                throw new ArgumentOutOfRangeException();
            if (string.IsNullOrEmpty(cultureName))
                throw new ArgumentNullException(nameof(cultureName));

            var culture = new CultureInfo(cultureName);

            IPredefinedMembersBuilder builder = null;

            //Look for registration ...
            var registration = registrations.FirstOrDefault(reg => reg.Value == value);
            if (registration == null)
                throw new ArgumentException(string.Format("'{0}' has no builder registred.", Enum.GetName(typeof(PredefinedMembers), value)));

            //Get Builder and initiate it
            builder = registration.Builder;
            builder.Setup(culture);

            //Build
            builder.Build();
            var list = builder.GetResult();

            return list;
        }
    }
}
