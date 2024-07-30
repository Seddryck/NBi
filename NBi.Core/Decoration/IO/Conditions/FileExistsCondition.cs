using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Conditions
{
    class FileExistsCondition : IDecorationCondition
    {
        private FileExistsConditionArgs Args { get; }

        public string Message { get; private set; } = string.Empty;

        public FileExistsCondition(FileExistsConditionArgs args) => Args = args;

        public bool Validate()
        {
            var fullPath = PathExtensions.CombineOrRoot(Args.BasePath, Args.FolderName.Execute() ?? string.Empty, Args.FileName.Execute() ?? string.Empty);

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
            => (File.Exists(fullPath), $"The file '{fullPath}' doesn't exists.");

        protected (bool, string) IsNotEmptyCondition(string fullPath)
            => (new FileInfo(fullPath).Length > 0, $"The file '{fullPath}' has a size of 0 byte.");
    }
}
