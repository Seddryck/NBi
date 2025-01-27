using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility;

public interface ITemplateEngine
{
    string Render(string template, IEnumerable<KeyValuePair<string, object>> variables);
}
