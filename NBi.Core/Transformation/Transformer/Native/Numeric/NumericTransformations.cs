using NBi.Core.Scalar.Casting;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractNumericTransformation : INativeTransformation
    {
        public AbstractNumericTransformation()
        { }

        public object Evaluate(object value)
        {
            switch (value)
            {
                case null: return EvaluateNull();
                case DBNull _: return EvaluateNull();
                case decimal numeric: return EvaluateNumeric(numeric);
                default: return EvaluateUncasted(value);
            }
        }

        private object EvaluateUncasted(object value)
        {
            if (value as string == "(null)")
                EvaluateNull();

            var caster = new NumericCaster();
            var numeric = caster.Execute(value);
            return EvaluateNumeric(numeric);
        }

        protected virtual object? EvaluateNull() => null;
        protected abstract decimal EvaluateNumeric(decimal numeric);
    }

    class NullToZero : AbstractNumericTransformation
    {
        protected override object EvaluateNull() => 0;
        protected override decimal EvaluateNumeric(decimal numeric) => numeric;
    }

    class NumericToCeiling : AbstractNumericTransformation
    {
        protected override decimal EvaluateNumeric(decimal numeric) => Math.Ceiling(numeric);
    }

    class NumericToFloor : AbstractNumericTransformation
    {
        protected override decimal EvaluateNumeric(decimal numeric) => Math.Floor(numeric);
    }

    class NumericToInteger : AbstractNumericTransformation
    {
        protected override decimal EvaluateNumeric(decimal numeric) => Math.Round(numeric, 0);
    }

    class NumericToRound : AbstractNumericTransformation
    {
        public IScalarResolver<int> Digits { get; }

        public NumericToRound(IScalarResolver<int> digits)
            => Digits = digits;

        protected override decimal EvaluateNumeric(decimal numeric) => Math.Round(numeric, Digits.Execute());
    }

    class NumericToClip : AbstractNumericTransformation
    {
        public IScalarResolver<decimal> Min { get; }
        public IScalarResolver<decimal> Max { get; }

        public NumericToClip(IScalarResolver<decimal> min, IScalarResolver<decimal> max)
            => (Min, Max) = (min, max);

        protected override decimal EvaluateNumeric(decimal numeric) => (numeric < Min.Execute()) ? Min.Execute() : (numeric > Max.Execute()) ? Max.Execute() : numeric;
    }

    abstract class AbstractNumericArithmetic : AbstractNumericTransformation
    {
        public IScalarResolver<decimal> Value { get; }

        public AbstractNumericArithmetic(IScalarResolver<decimal> value)
            => Value = value;
    }

    class NumericToAdd : AbstractNumericArithmetic
    {
        public IScalarResolver<int> Times { get; }

        public NumericToAdd(IScalarResolver<decimal> value, IScalarResolver<int> times)
            : base(value) => Times = times;

        public NumericToAdd(IScalarResolver<decimal> value)
            : this(value, new LiteralScalarResolver<int>(1)) { }

        protected override decimal EvaluateNumeric(decimal value)
            => value + (Value.Execute() * Times.Execute());
    }

    class NumericToSubtract : NumericToAdd
    {
        public NumericToSubtract(IScalarResolver<decimal> value, IScalarResolver<int> times)
            : base(value, times) { }

        public NumericToSubtract(IScalarResolver<decimal> value)
            : base(value) { }

        protected override decimal EvaluateNumeric(decimal value)
            => value - (Value.Execute() * Times.Execute());
    }

    class NumericToIncrement : NumericToAdd
    {
        public NumericToIncrement()
        : base(new LiteralScalarResolver<decimal>(1)) { }
    }

    class NumericToDecrement : NumericToSubtract
    {
        public NumericToDecrement()
        : base(new LiteralScalarResolver<decimal>(1)) { }
    }

    class NumericToMultiply : AbstractNumericArithmetic
    {
        public NumericToMultiply(IScalarResolver<decimal> value)
            : base(value) { }

        protected override decimal EvaluateNumeric(decimal value)
            => value * Value.Execute();
    }

    class NumericToDivide : AbstractNumericArithmetic
    {
        public NumericToDivide(IScalarResolver<decimal> value)
            : base(value) { }

        protected override decimal EvaluateNumeric(decimal value)
            => value / Value.Execute();
    }

    class NumericToInvert : AbstractNumericTransformation
    {
        public NumericToInvert()
        { }

        protected override decimal EvaluateNumeric(decimal value) => 1/value;
    }
}
