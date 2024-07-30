using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Predefined
{
    internal abstract class BaseBuilder : IPredefinedMembersBuilder
    {
        public CultureInfo Culture { get; protected set;} = CultureInfo.InvariantCulture;
        protected IEnumerable<string> Result { get; set; } = [];
        private bool isSetup = false;
        private bool isBuild = false;

        public void Setup(CultureInfo culture)
        {
            Culture = culture;
            Result = [];
            isBuild = false;
            isSetup = true;
        }       

        public void Build()
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

        protected string ToTitleCase(string value)
        {
            return Culture.TextInfo.ToTitleCase(value);
        }

        protected string ToUpperCase(string value)
        {
            return Culture.TextInfo.ToUpper(value);
        }

        protected string ToLowerCase(string value)
        {
            return Culture.TextInfo.ToLower(value);
        }

    }
}
