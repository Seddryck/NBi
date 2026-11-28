using NBi.Core.ResultSet.Combination;
using NBi.Model.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.ResultSet.Combination;

public class SequenceCombination
{
    public SequenceCombinationOperation Operation { get; set; }

    public List<SequenceDefinition> Sequences { get; set; } = new List<SequenceDefinition>();

    public bool SequencesSpecified { get => Sequences.Count > 0; set { } }

    public SequenceCombination() => Operation = SequenceCombinationOperation.CartesianProduct;
}
