﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Testing.Unit.Core.Assemblies.Resource
{
    class StaticKlass
    {
        
        public static string ExecuteStaticString(string paramString)
        {
            if (paramString == "MyString")
                return "Executed";
            return "Incorrect Parameters";
        }
    }
}
