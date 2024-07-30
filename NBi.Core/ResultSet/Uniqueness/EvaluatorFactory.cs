using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Uniqueness
{
    public class EvaluatorFactory
    {
        public virtual Evaluator Instantiate(ISettingsResultSet settings)
            => settings switch
            {
                SettingsOrdinalResultSet ordinal => new OrdinalEvaluator(ordinal),
                SettingsNameResultSet name => new NameEvaluator(name),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
