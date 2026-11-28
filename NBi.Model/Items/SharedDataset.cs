using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Model.Settings;
using NBi.Model.Constraints;

namespace NBi.Model.Items;

public class SharedDataset : Queryable, IReferenceFriendly
{
    public string Source { get; set; }

    public string Path { get; set; }

    public string Name { get; set; }

    public new List<QueryParameter> Parameters { get; set; }

    public SharedDataset()
    {
        Parameters = new List<QueryParameter>();
    }

    public new List<QueryParameter> GetParameters()
    {
        var list = Parameters;
        if (Default!=null)
            foreach (var param in Default.Parameters)
                if (!Parameters.Exists(p => p.Name == param.Name))
                    list.Add(param);

        return list;
    }

    public void AssignReferences(IEnumerable<Reference> references)
    {
        if (string.IsNullOrEmpty(Source))
            Source = Default.Report.Source;
        if (string.IsNullOrEmpty(Path))
            Path = Default.Report.Path;

        if (!string.IsNullOrEmpty(Source) && Source.StartsWith("@"))
            Source = InitializeFromReferences(references, Source, "source");
        if (!string.IsNullOrEmpty(Path) && Path.StartsWith("@"))
            Path = InitializeFromReferences(references, Path, "path");
    }

    protected virtual string InitializeFromReferences(IEnumerable<Reference> references, string refName, string attribute)
    {
        if (refName.StartsWith("@"))
            refName = refName.Substring(1);

        var refChoice = GetReference(references, refName);

        if (refChoice.Report == null)
            throw new NullReferenceException(string.Format("A reference named '{0}' has been found, but no element 'report' has been defined", refName));

        if (attribute=="source")
            return refChoice.Report.Source;
        if (attribute=="path")
            return refChoice.Report.Path;
        throw new ArgumentOutOfRangeException();
    }

    protected Reference GetReference(IEnumerable<Reference> references, string value)
    {
        if (references == null || references.Count() == 0)
            throw new InvalidOperationException("No reference has been defined for this constraint");

        var refChoice = references.FirstOrDefault(r => r.Name == value);
        if (refChoice == null)
            throw new IndexOutOfRangeException(string.Format("No reference named '{0}' has been defined.", value));
        return refChoice;
    }
}
