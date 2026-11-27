using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Model.Items;
using NBi.Model.Items.ResultSet;
using NBi.Model.Settings;
using NBi.Model.Constraints.Comparer;
using NBi.Model.Items.Calculation;
using NBi.Core.Evaluate;
using System;

namespace NBi.Model.Constraints;

public class NoRows : AbstractConstraint
{
    public List<IColumnAlias> Aliases { get => InternalAliases.ToList<IColumnAlias>(); }

    public List<Alias> InternalAliases
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }

    //Receiving the order 2 when readonly attribute is activated
    [Obsolete("Use InternalAlias in place of InternalAliasOld")]
    public List<Alias> InternalAliasesOld
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }


    public List<Expression> Expressions { get; set; }

    private List<Alias> internalAliases;

    public SinglePredication Predication { get; set; }

    public CombinationPredication Combination { get; set; }

    public NoRows()
    {
        internalAliases = new List<Alias>();
        Expressions = new List<Expression>();
    }
}
