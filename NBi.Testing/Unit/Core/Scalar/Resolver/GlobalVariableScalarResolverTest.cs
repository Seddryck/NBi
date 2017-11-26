using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Resolver
{
    public class GlobalVariableScalarResolverTest
    {
        [Test]
        public void Execute_ExistingVariable_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new TestVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10*10"))) },
                { "otherVar" , new TestVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables);
            var resolver = new GlobalVariableScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(100));
        }

        [Test]
        public void Execute_ExistingVariableWrongType_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new TestVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("(10*10).ToString()"))) },
                { "otherVar" , new TestVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables);
            var resolver = new GlobalVariableScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(100));
        }

        [Test]
        public void Execute_ExistingVariableWrongTypeDateTime_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, ITestVariable>()
            {
                { "myVar" , new TestVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("\"2017-05-12\""))) },
                { "otherVar" , new TestVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new GlobalVariableScalarResolverArgs("myVar", globalVariables);
            var resolver = new GlobalVariableScalarResolver<DateTime>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(new DateTime(2017,5,12)));
        }
    }
}
