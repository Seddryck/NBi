using NBi.Core.Calculation.Asserting;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering;

class PredicationFilter : BaseFilter
{
    private IPredication Predication { get; }

    public PredicationFilter(IPredication predication, Context context)
        : base(context) => Predication = predication;

    protected override bool RowApply(Context context)
        => Predication.Execute(context);

    public override string Describe()
        => $"{Predication.GetType().Name}";
}
