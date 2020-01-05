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

        public string Message => throw new NotImplementedException();

        public FileExistsCondition(FileExistsConditionArgs args) => Args = args;

        public bool Validate()
        {
            var fullPath = PathExtensions.CombineOrRoot(Args.BasePath, Args.FolderName.Execute(), Args.FileName.Execute());

            var conditions = new List<Func<string, bool>>() { ExistsCondition };
            if (Args.NotEmpty.Execute())
                conditions.Add(IsNotEmptyCondition);

            var result = true;
            var enumerator = conditions.GetEnumerator();
            while (result && enumerator.MoveNext())
                result = enumerator.Current.Invoke(fullPath);
            return result;
        }

        protected bool ExistsCondition(string fullPath)
            => File.Exists(fullPath);

        protected bool IsNotEmptyCondition(string fullPath)
            => new FileInfo(fullPath).Length > 0;
    }
}
