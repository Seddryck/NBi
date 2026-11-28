using NBi.Model.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Summarization;

public class Summarize : AlterationDefinition
{
    public Aggregation Aggregation { get; set; }

    public GroupBy GroupBy { get; set; }
}
