using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation;

public class CombinationPredication : AbstractPredication
{
    public CombinationOperator Operator { get; set; }

    public bool Not { get; set; }

    public List<SinglePredication> Predications { get; set; } = [];
}
