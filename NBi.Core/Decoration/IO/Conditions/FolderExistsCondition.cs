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
    class FolderExistsCondition : IDecorationCondition
    {
        private FolderExistsConditionArgs Args { get; }

        public string Message => throw new NotImplementedException();

        public FolderExistsCondition(FolderExistsConditionArgs args) => Args = args;

        public bool Validate()
        {
            var path = PathExtensions.CombineOrRoot(Args.BasePath, Args.FolderPath.Execute());
            var fullPath = PathExtensions.CombineOrRoot(path, Args.FolderName.Execute());

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
            => Directory.Exists(fullPath);

        protected bool IsNotEmptyCondition(string fullPath)
            => !string.IsNullOrEmpty(Directory.EnumerateFiles(fullPath).FirstOrDefault());
    }
}
