using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Conditions;

class FolderExistsCondition : IDecorationCondition
{
    private FolderExistsConditionArgs Args { get; }

    public string Message { get; private set; } = string.Empty;

    public FolderExistsCondition(FolderExistsConditionArgs args) => Args = args;

    public bool Validate()
    {
        var path = PathExtensions.CombineOrRoot(Args.BasePath, Args.FolderPath.Execute() ?? string.Empty);
        var fullPath = PathExtensions.CombineOrRoot(path, Args.FolderName.Execute() ?? string.Empty);

        var conditions = new List<Func<string, (bool, string)>>() { ExistsCondition };
        if (Args.NotEmpty.Execute())
            conditions.Add(IsNotEmptyCondition);

        var result = true;
        var enumerator = conditions.GetEnumerator();
        while (result && enumerator.MoveNext())
            (result, Message) = enumerator.Current.Invoke(fullPath);
        return result;
    }

    protected (bool, string) ExistsCondition(string fullPath)
        => (Directory.Exists(fullPath), $"The file '{fullPath}' doesn't exists.");

    protected (bool, string) IsNotEmptyCondition(string fullPath)
        => (!string.IsNullOrEmpty(Directory.EnumerateFiles(fullPath).FirstOrDefault()), $"The directory '{fullPath}' doesn't contain any file.");
}
