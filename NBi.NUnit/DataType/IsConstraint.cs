using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Structure;
using NUnit.Framework.Constraints;
using NBi.Core.DataType;
using NBi.NUnit.Structure;

namespace NBi.NUnit.DataType
{
    public class IsConstraint : NBiConstraint
    {
        protected DataTypeInfo expected;

        private DataTypeInfo Actual
        {
            get { return base.actual as DataTypeInfo; }
        }

        public IDataTypeDiscoveryCommand Command { get; protected set; }

        /// <summary>
        /// Construct a ExistsConstraint
        /// </summary>
        public IsConstraint(string expected)
        {
            var factory = new DataTypeInfoFactory();
            this.expected = factory.Instantiate(expected);
        }

        public override bool Matches(object actual)
        {
            if (actual is IDataTypeDiscoveryCommand)
                return Process((IDataTypeDiscoveryCommand)actual);
            else if (actual is DataTypeInfo)
            {
                this.actual = actual;
                var result = Actual.Name == expected.Name;
                result &= expected is ILength && Actual is ILength && ((ILength)expected).Length.HasValue ? ((ILength)Actual).Length.Value == ((ILength)expected).Length.Value : result;
                result &= expected is IScale && Actual is IScale && ((IScale)expected).Scale.HasValue ? ((IScale)Actual).Scale.Value == ((IScale)expected).Scale.Value : result;
                result &= expected is IPrecision && Actual is IPrecision && ((IPrecision)expected).Precision.HasValue ? ((IPrecision)Actual).Precision.Value == ((IPrecision)expected).Precision.Value : result;
                return result;
            }
            else
                throw new ArgumentException();
        }

        protected bool Process(IDataTypeDiscoveryCommand actual)
        {
            Command = actual;
            DataTypeInfo type = Command.Execute();
            return this.Matches(type);
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            var description = new DescriptionDataTypeHelper();
            var filterExpression = description.GetFilterExpression(Command.Description.Filters.Where(f => f.Target != Command.Description.Target));
            var targetExpression = description.GetTargetExpression(Command.Description.Target);
            var captionExpression = Command.Description.Filters.Single(f => f.Target == Command.Description.Target).Caption;

            writer.WritePredicate(string.Format("the type of {0} '{1}' ({2}) is '{3}'"
                        , targetExpression
                        , captionExpression
                        , filterExpression
                        , expected.ToString()));
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            //IF actual is not empty it means we've an issue with Casing or a space at the end
            if (actual == null)
                writer.WriteActualValue(new WriterHelper.NothingFoundMessage());
            else
            {
                var result = Actual.Name;
                result += expected is ILength && Actual is ILength && ((ILength)expected).Length.HasValue ? "(" + ((ILength)Actual).Length.Value : "";
                result += expected is IPrecision && Actual is IPrecision && ((IPrecision)expected).Precision.HasValue ? "(" + ((IPrecision)Actual).Precision.Value : "";
                result += expected is IScale && Actual is IScale && ((IScale)expected).Scale.HasValue ? "," + ((IScale)Actual).Scale.Value : "";
                result += result.Contains("(") ? ")" : "";

                writer.WriteActualValue(result);
            }
        }
    }
}
