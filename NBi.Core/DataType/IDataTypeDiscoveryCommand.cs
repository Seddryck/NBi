﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NBi.Core.DataType;

public interface IDataTypeDiscoveryCommand
{
    CommandDescription Description { get; }
    DataTypeInfo? Execute();
}
