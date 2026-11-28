using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items;

public class FileSource
{
    [Obsolete("Use 'Path' instead of 'Value'")]
    public string? Value { get => Path; set => Path = value; }

    public virtual string? Path { get; set; }

    public virtual Parser? Parser { get; set; }

    public virtual IfMissing? IfMissing { get; set; }

    public virtual bool IsBasic() => Parser == null && IfMissing==null;
    public virtual bool IsEmpty() => string.IsNullOrEmpty(Path);
}
