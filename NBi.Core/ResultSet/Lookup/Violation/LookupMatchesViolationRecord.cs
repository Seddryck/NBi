using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup.Violation;

public class LookupMatchesViolationRecord : Dictionary<IResultColumn, LookupMatchesViolationData>
{ }
