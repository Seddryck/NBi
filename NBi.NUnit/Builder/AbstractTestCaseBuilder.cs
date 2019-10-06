using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;
using NBi.Framework;
using System.Collections.Generic;
using NBi.Core.Variable;
using System.Diagnostics;
using NBi.Core;
using NBi.Core.Injection;
using NBi.Core.Configuration;
using NBi.Extensibility;

namespace NBi.NUnit.Builder
{
    abstract class AbstractTestCaseBuilder : ITestCaseBuilder
    {
        protected object SystemUnderTest { get; set; }
        protected NBiConstraint Constraint { get; set; }
        protected IConfiguration Configuration { get; private set; }
        
        protected IDictionary<string, ITestVariable> Variables { get; private set; }
        protected ServiceLocator ServiceLocator { get; private set; }

        protected bool isSetup;
        protected bool isBuild;

        internal void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            Setup(sutXml, ctrXml, null, null, null);
        }

        public virtual void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, IConfiguration config, IDictionary<string, ITestVariable> variables, ServiceLocator serviceLocator)
        {
            Configuration = config ?? Core.Configuration.Configuration.Default;
            Variables = variables ?? new Dictionary<string, ITestVariable>();
            ServiceLocator = serviceLocator;
            BaseSetup(sutXml, ctrXml);
            SpecificSetup(sutXml, ctrXml);
            isSetup = true;
        }

        protected object EvaluatePotentialVariable(object reference)
        {
            if (!(reference is string))
                return reference;

            var value = (reference as string).Trim();
            if (!value.StartsWith("@"))
                return reference;
            else
                value = value.Substring(1);

            if (!Variables.ContainsKey(value))
                throw new NBiException($"The variable named '{value}' is not defined.");

            var variable = Variables[value];

            var stopWatch = new Stopwatch();
            var isFirstEvaluation = !variable.IsEvaluated();
            stopWatch.Start();
            var output = variable.GetValue();
            if (isFirstEvaluation)
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed for evaluation of variable '{value}': {stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Variable '{value}' evaluated to: {output}");
            }
            return output;
        }

        protected abstract void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);
        protected abstract void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException("The method Build must be preceded by a call to method Setup");

            BaseBuild();
            SpecificBuild();
            Constraint.Configuration = Configuration;
            Constraint.GlobalVariables = Variables;

            isBuild = true;
        }

        protected abstract void BaseBuild();
        protected abstract void SpecificBuild();

        public object GetSystemUnderTest()
        {
            if (!isBuild)
                throw new InvalidOperationException("The method GetSystemUnderTest must be preceded by a call to method Build");

            return SystemUnderTest;
        }

        public NBiConstraint GetConstraint()
        {
            if (!isBuild)
                throw new InvalidOperationException("The method GetConstraint must be preceded by a call to method Build");

            return Constraint;
        }

    }
}
