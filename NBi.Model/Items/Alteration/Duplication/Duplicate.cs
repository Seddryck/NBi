using NBi.Core.ResultSet;
using NBi.Model.Constraints.Comparer;
using NBi.Model.Items.Calculation;
using NBi.Model.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Duplication;

public class Duplicate : AlterationDefinition
{
    public SinglePredication Predication { get; set; }

    public string Times { get; set; } = "1";

    public List<Output> Outputs { get; set; } = new List<Output>();
}
