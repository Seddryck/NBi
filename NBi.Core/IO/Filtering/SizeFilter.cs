using NBi.Core.Calculation.Predicate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.IO.Filtering
{
    class SizeFilter : IPropertyFilter
    {
        private readonly IPredicate predicate;

        public SizeFilter(IPredicate predicate) => this.predicate = predicate;

        public bool Execute(FileInfo fileInfo)
            => predicate.Execute(fileInfo.Length);
    }
}
