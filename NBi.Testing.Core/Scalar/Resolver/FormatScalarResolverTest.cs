using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver
{
    public class FormatScalarResolverTest
    {
        [Test]
        public void Execute_ExistingNumericVariable_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, IVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10*10"))) },
                { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
            };
            var args = new FormatScalarResolverArgs("Twenty = {@otherVar:#0.00}?", new Context(globalVariables));
            var resolver = new FormatScalarResolver(args, new ServiceLocator());
            Assert.That(resolver.Execute(), Is.EqualTo("Twenty = 20.00?"));
        }

        [Test]
        [SetCulture("en-us")]
        public void Execute_VariableWithNativeTransformation_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, IVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("new DateTime(2019, 6, 1)"))) },
            };
            var args = new FormatScalarResolverArgs("First of May was a {@myVar | dateTime-to-previous-month:dddd}", new Context(globalVariables));
            var resolver = new FormatScalarResolver(args, new ServiceLocator());
            var text = resolver.Execute();
            Assert.That(text, Is.EqualTo($"First of May was a Wednesday"));
        }

        [Test]
        [SetCulture("fr-fr")]
        public void Execute_VariableWithNativeTransformation_IndependantOfLocalCulture()
        {
            var globalVariables = new Dictionary<string, IVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("new DateTime(2019, 6, 1)"))) },
            };
            var args = new FormatScalarResolverArgs("First of May was a {@myVar | dateTime-to-previous-month:dddd}", new Context(globalVariables));
            var resolver = new FormatScalarResolver(args, new ServiceLocator());
            var text = resolver.Execute();
            Assert.That(text, Is.EqualTo($"First of May was a Wednesday"));
        }

        [Test]
        public void Execute_VariableWithTwoNativeTransformations_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, IVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("new DateTime(2019, 6, 12)"))) },
            };
            var args = new FormatScalarResolverArgs("First day of the month before was a {@myVar | dateTime-to-previous-month | dateTime-to-first-of-month:dddd}", new Context(globalVariables));
            var resolver = new FormatScalarResolver(args, new ServiceLocator());
            var text = resolver.Execute();
            Assert.That(text, Is.EqualTo($"First day of the month before was a Wednesday"));
        }

        [Test]
        public void Execute_VariableWithNativeTransformationParametrized_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, IVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10*10"))) },
            };
            var args = new FormatScalarResolverArgs("My clipped value is {@myVar | numeric-to-clip(20, 80):##.00}", new Context(globalVariables));
            var resolver = new FormatScalarResolver(args, new ServiceLocator());
            var text = resolver.Execute();
            Assert.That(text, Is.EqualTo($"My clipped value is 80.00"));
        }

        [Test]
        [SetCulture("fr-fr")]
        public void Execute_ExistingDateTimeVariable_CorrectEvaluation()
        {
            var globalVariables = new Dictionary<string, IVariable>()
            {
                { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("new DateTime(2018,1,1)"))) },
            };
            var args = new FormatScalarResolverArgs("First day of 2018 is a {@myVar:dddd}", new Context(globalVariables));
            var resolver = new FormatScalarResolver(args, new ServiceLocator());
            Assert.That(resolver.Execute(), Is.EqualTo("First day of 2018 is a Monday"));
        }
    }
}
