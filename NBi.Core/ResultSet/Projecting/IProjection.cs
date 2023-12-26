﻿using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Projecting
{
    public interface IProjection
    {
        IResultRow Execute(IResultRow resultSet);
    }
}
