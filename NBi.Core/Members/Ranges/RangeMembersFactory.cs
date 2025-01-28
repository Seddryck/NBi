using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges;

public class RangeMembersFactory
{
    private readonly ICollection<BuilderRegistration> registrations;

    public RangeMembersFactory()
    {
        registrations = [];
        RegisterDefaults();
    }

    private void RegisterDefaults()
    {
        Register([typeof(IIntegerRange), typeof(IPatternDecorator)]
            , new CompositeBuilder(
                new IntegerRangeBuilder()
                , [new PatternDecoratorBuilder()]
                ));
        Register(typeof(IIntegerRange), new IntegerRangeBuilder());
        Register(typeof(IDateRange), new DateRangeBuilder());
    }

    /// <summary>
    /// Register a new builder for corresponding types. If a builder was already existing for this association, it's replaced by the new one
    /// </summary>
    /// <param name="sutType">Type of System Under Test</param>
    /// <param name="ctrType">Type of Constraint</param>
    /// <param name="builder">Instance of builder deicated for these types of System Under Test and Constraint</param>
    public void Register(Type rangeType, IRangeMembersBuilder builder)
    {
        Register([rangeType], builder);
    }

    /// <summary>
    /// Register a new builder for corresponding types. If a builder was already existing for this association, it's replaced by the new one
    /// </summary>
    /// <param name="sutType">Type of System Under Test</param>
    /// <param name="ctrType">Type of Constraint</param>
    /// <param name="builder">Instance of builder deicated for these types of System Under Test and Constraint</param>
    public void Register(IEnumerable<Type> rangeTypes, IRangeMembersBuilder builder)
    {
        if (IsHandling(rangeTypes))
            registrations.First(reg => reg.Match(rangeTypes)).Builder = builder;
        else
            registrations.Add(new BuilderRegistration(rangeTypes, builder));
    }

    private bool IsHandling(IEnumerable<Type> rangeTypes)
        => registrations.Any(reg => reg.Match(rangeTypes));

    private class BuilderRegistration
    {
        public IEnumerable<Type> Types { get; set; }
        public IRangeMembersBuilder Builder { get; set; }

        public BuilderRegistration(IEnumerable<Type> rangeTypes, IRangeMembersBuilder builder)
        {
            Types = rangeTypes;
            Builder = builder;
        }

        public BuilderRegistration(Type rangeType, IRangeMembersBuilder builder)
        {
            Types = [rangeType];
            Builder = builder;
        }

        public bool Match(IEnumerable<Type> rangeTypes)
        {
            return rangeTypes.ToList().TrueForAll(rt => this.Match(rt));
        }

        public bool Match(Type rangeType)
        {
            return Types.ToList().TrueForAll(
                type => 
                    type.IsAssignableFrom(rangeType)
               );
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
            throw new ArgumentNullException(nameof(range));

        //Look for registration ...
        var registration = registrations.FirstOrDefault(reg => reg.Match(range.GetType())) ?? throw new ArgumentException(string.Format("'{0}' has no builder registred.", range.GetType().Name, "range"));

        //Get Builder and initiate it
        var builder = registration.Builder;
        builder.Setup(range);

        //Build
        builder.Build();
        var list = builder.GetResult();

        return list;
    }
}
