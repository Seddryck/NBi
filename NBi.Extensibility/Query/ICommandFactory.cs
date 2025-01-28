using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Extensibility.Query;

public interface ICommandFactory
{
    bool CanHandle(IClient client);
    ICommand Instantiate(IClient client, IQuery query, ITemplateEngine engine);
}
