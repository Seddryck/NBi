using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public class AssemblyQueryResolverArgs : BaseQueryResolverArgs
{
    private readonly string path;
    private readonly string className;
    private readonly string methodName;
    private readonly bool isStatic;
    private readonly IDictionary<string, object> methodParameters;

    public string Path { get => path; }
    public string ClassName { get => className; }
    public string MethodName { get => methodName; }
    public bool IsStatic { get => isStatic; }
    public IDictionary<string, object> MethodParameters { get => methodParameters; }

    public AssemblyQueryResolverArgs(string path, string className, string methodName, bool isStatic, IDictionary<string, object> methodParameters, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
        : base(connectionString, parameters, variables, timeout)
    {
        this.path = path;
        this.className = className;
        this.methodName = methodName;
        this.isStatic = isStatic;
        this.methodParameters = methodParameters;
    }
}
