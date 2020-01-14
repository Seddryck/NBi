using System;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NUnit.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.Scalar.Resolver;

namespace NBi.NUnit.Scoring
{
    public class ScoreConstraint : NBiConstraint
    {
        protected decimal Threshold { get; private set; }
        protected decimal Actual { get; private set; }
        protected bool Success { get; private set; }

        public ScoreConstraint(decimal threshold)
        {
            Threshold = threshold;
        }

        private IScoreMessageFormatter failure;
        protected IScoreMessageFormatter Failure
        {
            get
            {
                if (failure == null)
                    failure = BuildFailure();
                return failure;
            }
        }

        protected virtual IScoreMessageFormatter BuildFailure()
        {
            var factory = new ScoreMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            msg.Initialize(Actual, Threshold, Success);
            return msg;
        }
     
        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An IResultSetService or ResultSet</param>
        /// <returns>true, if the execution of the actual IResultSetService returns a ResultSet identical to the content of the expected ResultSet</returns>
        public override bool Matches(object actual)
        {
            if (actual is IScalarResolver<decimal>)
                return Process((IScalarResolver<decimal>)actual);
            if (actual is decimal)
                return doMatch((decimal)actual);
            else
                throw new ArgumentException($"The type of the actual object is '{actual.GetType().Name}' and is not supported for a constraint of type '{this.GetType().Name}'. Use a IScalarResolver.", nameof(actual));
        }

        protected bool doMatch(decimal value)
        {
            Actual = value;
            Success = Actual >= Threshold;

            if (Success && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
                Assert.Pass(Failure.RenderMessage());

            return Success;
        }

        public bool Process(IScalarResolver<decimal> actual) => Matches(actual.Execute());

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine();
            writer.WriteLine(Failure.RenderExpected());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine();
            writer.WriteLine(Failure.RenderActual());
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(Failure.RenderMessage());
            else
            {
                writer.WritePredicate("Score is not sufficient.");
                writer.WriteLine();
                writer.WriteLine();
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine(Failure.RenderMessage());
            }
        }
    }
}
