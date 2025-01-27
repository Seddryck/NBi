using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration;

[TypeConverter(typeof(DurationConverter))]
public interface IDuration
{ }
