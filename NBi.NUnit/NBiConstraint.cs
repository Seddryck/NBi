using NBi.Core.Configuration;
using NBi.Core.Variable;
using NUnit.Framework.Constraints;

namespace NBi.NUnit;

public abstract class NBiConstraint : Constraint
{
    protected object? actual;

    public IConfiguration Configuration { get; set; }
    public IDictionary<string, IVariable> GlobalVariables { get; set; }

    public override string Description => DisplayName;

    public sealed override ConstraintResult ApplyTo<TActual>(TActual value)
    {
        actual = value;
        return new NBiConstraintResult(this, value, Matches(value!));
    }

    public abstract bool Matches(object actual);

    public virtual void WriteDescriptionTo(MessageWriter writer)
        => writer.Write(Description);

    public virtual void WriteActualValueTo(MessageWriter writer)
        => writer.WriteActualValue(actual);

    public virtual void WriteMessageTo(MessageWriter writer)
    {
        writer.Write("  Expected: ");
        WriteDescriptionTo(writer);
        writer.WriteLine();
        writer.Write("  But was:  ");
        WriteActualValueTo(writer);
        writer.WriteLine();
    }

    private sealed class NBiConstraintResult(NBiConstraint constraint, object? value, bool success)
        : ConstraintResult(constraint, value, success)
    {
        public override void WriteMessageTo(MessageWriter writer)
            => constraint.WriteMessageTo(writer);

        public override void WriteActualValueTo(MessageWriter writer)
            => constraint.WriteActualValueTo(writer);
    }
}
