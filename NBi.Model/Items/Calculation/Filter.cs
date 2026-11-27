using NBi.Core.Evaluate;
using NBi.Model.Items.Alteration;
using NBi.Model.Items.Calculation;
using NBi.Model.Items.Calculation.Ranking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation;

public class Filter : AlterationDefinition
{
    public IReadOnlyCollection<IColumnAlias> Aliases
    {
        get
        {
            return new ReadOnlyCollection<IColumnAlias>(internalAliases.Cast<IColumnAlias>().ToList());
        }
    }

    public List<Alias> InternalAliases
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }

    [Obsolete("Use InternalAlias in place of InternalAliasOld")]
    public List<Alias> InternalAliasesOld
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }

    private List<Alias> internalAliases;

    public Expression Expression { get; set; }

    public SinglePredication Predication { get; set; }

    public RankingDefinition Ranking { get; set; }
    public Unique Uniqueness { get; set; }

    public CombinationPredication Combination { get; set; }

    //public RankingDefinition Ranking { get; set; }

    public Filter()
    {
        internalAliases = new List<Alias>();
    }
}
