using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Evaluate;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Evaluate
{
    [TestFixture]
    public class RowValidatorTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion


        [Test]
        public void Execute_BasicMathValid_True()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=4+4", 8));

            var validator = new RowValidator();
            var result = validator.Execute(new Dictionary<string, object>(), expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Execute_BasicMathInvalid_False()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=4+4", 0));

            var validator = new RowValidator();
            var result = validator.Execute(new Dictionary<string, object>(), expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Execute_VariableValid_True()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=X+4", 9));
            var variables =  new Dictionary<string, object>();
            variables["X"] = 5;

            var validator = new RowValidator();
            var result = validator.Execute(variables, expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Execute_VariableInvalid_False()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=X+4", 0));
            var variables = new Dictionary<string, object>();
            variables["X"] = 5;


            var validator = new RowValidator();
            var result = validator.Execute(variables, expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Execute_MathVariablesValid_True()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=Abs(x*y)", 10));
            var variables = new Dictionary<string, object>();
            variables["x"] = -5;
            variables["y"] = 2;

            var validator = new RowValidator();
            var result = validator.Execute(variables, expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Execute_MathVariablesInvalid_False()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=Abs(x*y)", 0));
            var variables = new Dictionary<string, object>();
            variables["x"] = -5;
            variables["y"] = 2;

            var validator = new RowValidator();
            var result = validator.Execute(variables, expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Execute_MathVariablesToleranceValid_True()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=Abs(x*y)", 9, 2));
            var variables = new Dictionary<string, object>();
            variables["x"] = -5;
            variables["y"] = 2;

            var validator = new RowValidator();
            var result = validator.Execute(variables, expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Execute_MathVariablesToleranceInvalid_False()
        {
            var expressions = new List<ValuedExpression>();
            expressions.Add(new ValuedExpression("=Abs(x*y)", 0, 2));
            var variables = new Dictionary<string, object>();
            variables["x"] = -5;
            variables["y"] = 2;

            var validator = new RowValidator();
            var result = validator.Execute(variables, expressions).Aggregate<ExpressionEvaluationResult, bool>(true, (total, r) => total && r.IsValid);

            Assert.That(result, Is.False);
        }
    }
}
