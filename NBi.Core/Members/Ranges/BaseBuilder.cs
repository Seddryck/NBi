using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges
{
    internal abstract class BaseBuilder : IRangeMembersBuilder
    {
        protected IRange? Range { get; set; }
        protected IEnumerable<string> Result { get; set; } = [];
        private bool isSetup = false;
        private bool isBuild = false;

        public virtual void Setup(IRange range)
        {
            Result = [];
            Range = range;
            isBuild = false;
            isSetup = true;
        }       

        public virtual void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException();
            InternalBuild();
            isBuild = true;
        }

        protected abstract void InternalBuild();

        public IEnumerable<string> GetResult()
        {
            if (!isBuild)
                throw new InvalidOperationException();
            return Result;
        }
    }
}
