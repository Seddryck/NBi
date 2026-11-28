using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Items.Ranges;
using NBi.Model.Settings;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public class Ordered : AbstractConstraint
{
    public override Default? Default
    {
        get { return base.Default; }
        set
        {
            base.Default = value;
            if (Query != null)
                Query.Default = value;
        }
    }

    public bool Descending { get; set; }

    public Order Rule { get; set; }

    public List<object> Definition { get; set; }

    /// <summary>
    /// This property is there to control the serialization of the Definition object. If there is no definition (or an empty one), the field will not be part the output.
    /// </summary>
    /// <value>Whether the definition contains any elements.</value>
    public bool DefinitionSpecified
    {
        get
        {
            return !(Definition == null || Definition.Count == 0);
        }
        set
        {
            return;
        }
    }

    public Query? Query { get; set; }

    public Ordered()
    {
        Definition = [];
    }

    public enum Order
    {
        Alphabetical = 0,
        Chronological = 1,
        Numerical = 2,
        Specific = 3
    }
}
