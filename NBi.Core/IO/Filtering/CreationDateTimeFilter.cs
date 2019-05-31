using NBi.Core.Calculation.Predicate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.IO.Filtering
{
    class CreationDateTimeFilter : IPropertyFilter
    {
        private readonly IPredicate predicate;
        private readonly Func<FileInfo, bool> execute;

        public CreationDateTimeFilter(IPredicate predicate, bool isUtc)
        {
            this.predicate = predicate;
            if (isUtc)
                execute = (FileInfo fileInfo) => predicate.Execute(fileInfo.CreationTimeUtc);
            else
                execute = (FileInfo fileInfo) => predicate.Execute(fileInfo.CreationTime);
        }

        public bool Execute(FileInfo fileInfo)
            => predicate.Execute(fileInfo.CreationTimeUtc);
    }
}
