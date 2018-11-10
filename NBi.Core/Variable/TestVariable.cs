using Microsoft.CSharp;
using NBi.Core.Scalar;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class TestVariable : ITestVariable
    {
        private object value;
        private bool isEvaluated;
        private readonly IScalarResolver<object> resolver;
        
        public TestVariable(IScalarResolver<object> resolver)
        {
            this.resolver = resolver;
        }

        public object GetValue()
        {
            
            if (!IsEvaluated())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                value = resolver.Execute();
                isEvaluated = true;

                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Time needed for evaluation of the variable: {stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");

                var invariantCulture = new CultureFactory().Invariant;
                var msg = $"Variable evaluated to: {value}";
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, msg.ToString(invariantCulture));
            }

            return value;
        }

        public bool IsEvaluated()
        {
            return isEvaluated;
        }
    }
}
