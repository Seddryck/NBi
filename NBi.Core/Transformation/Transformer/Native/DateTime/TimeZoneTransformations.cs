using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class UtcToLocal : AbstractDateTimeTransformation
    {
        public IScalarResolver<string> TimeZoneLabel { get; }

        public UtcToLocal(IScalarResolver<string> timeZoneLabel)
        {
            TimeZoneLabel = timeZoneLabel;
        }

        protected override object EvaluateDateTime(DateTime value) =>
            TimeZoneInfo.ConvertTimeFromUtc(value, InstantiateTimeZoneInfo(TimeZoneLabel.Execute()));

        protected TimeZoneInfo InstantiateTimeZoneInfo(string label)
        {
            var zones = TimeZoneInfo.GetSystemTimeZones();
            var zone = zones.SingleOrDefault(z => z.Id == label)
                ?? zones.SingleOrDefault(z => Tokenize(z.DisplayName).Contains(label.Replace(" ", "")));

            return zone ?? throw new ArgumentOutOfRangeException($"TimeZone '{label}' is not existing on this computer.");
        }

        private string[] Tokenize(string label) =>
            label.Replace("(", ",")
            .Replace(")", ",")
            .Replace(":", ",")
            .Replace(" ", "")
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    class LocalToUtc : UtcToLocal
    {
        public LocalToUtc(IScalarResolver<string> timeZoneLabel)
            : base(timeZoneLabel)
        { }

        protected override object EvaluateDateTime(DateTime value) =>
            TimeZoneInfo.ConvertTimeToUtc(value, InstantiateTimeZoneInfo(TimeZoneLabel.Execute()));
    }
}
