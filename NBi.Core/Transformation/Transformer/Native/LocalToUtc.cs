using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class LocalToUtc : UtcToLocal
    {
        private readonly string timeZoneLabel;

        public LocalToUtc(string timeZoneLabel)
            : base(timeZoneLabel)
        { }

        protected override object EvaluateDateTime(DateTime value) =>
            TimeZoneInfo.ConvertTimeToUtc(value, InstantiateTimeZoneInfo(TimeZoneLabel));
    }
}
