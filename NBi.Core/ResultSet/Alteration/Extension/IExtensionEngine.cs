﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
    public interface IExtensionEngine
    {
        ResultSet Execute(ResultSet rs);
    }
}
