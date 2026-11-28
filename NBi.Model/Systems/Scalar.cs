using System.Collections.Generic;
using System.Linq;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Constraints;
using NBi.Core.ResultSet;
using NBi.Model.Items.ResultSet;
using System.IO;
using NBi.Model.Items.Alteration;

namespace NBi.Model.Systems;

public class Scalar : AbstractSystemUnderTest
{
    public virtual QueryScalar Query { get; set; }

    public override BaseItem BaseItem => Query;

    public override ICollection<string> GetAutoCategories() => new List<string>() { "Scalar" };
}
