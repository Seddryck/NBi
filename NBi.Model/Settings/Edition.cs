using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NBi.Model.Settings;

public class Edition
{
    public string Author { get; set; }
    public string DateTime { get; set; }
    public List<Update> Updates { get; set; }
}
