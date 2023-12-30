using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public class RowValueExtractor
    {
        private ServiceLocator ServiceLocator { get; }

        public RowValueExtractor(ServiceLocator serviceLocator)
            => (ServiceLocator) = (serviceLocator);

        public object? Execute(Context context, IColumnIdentifier identifier)
        {
            if (context.CurrentRow is null)
                throw new InvalidOperationException();

            if (identifier is ColumnOrdinalIdentifier ordinalId)
            {
                var ordinal = ordinalId.Ordinal;
                if (ordinal <= context.CurrentRow.Parent.ColumnCount)
                    return context.CurrentRow.ItemArray[ordinal] ?? throw new ArgumentOutOfRangeException();
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {context.CurrentRow.Parent.ColumnCount} columns.");
            }

            if (identifier is ColumnNameIdentifier nameId)
            {
                var name = nameId.Name;
                var alias = context.Aliases?.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
                if (alias != null)
                    return context.CurrentRow.ItemArray[alias.Column] ?? throw new ArgumentOutOfRangeException();

                var expression = context.Expressions?.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
                if (expression != null)
                {
                    var result = EvaluateExpression(expression, context);
                    var expColumnName = $"exp::{name}";
                    if (!context.CurrentRow.Parent.ContainsColumn(expColumnName))
                        context.CurrentRow.Parent.AddColumn(expColumnName);
                    context.CurrentRow[expColumnName] = result;
                    return result;
                }

                var column = context.CurrentRow.Parent.GetColumn(name);
                if (column != null)
                    return context.CurrentRow[column.Name];

                var existingNames = context.CurrentRow.Parent.Columns.Select(x => x.Name)
                    .Union(context.Aliases!.Select(x => x.Name)
                    .Union(context.Expressions!.Select(x => x.Name)));

                throw new ArgumentException($"The value '{name}' is not recognized as a column position, a column name, a column alias or an expression. Possible arguments are: '{string.Join("', '", existingNames.ToArray())}'");
            }
            throw new ArgumentException();
        }

        protected object? EvaluateExpression(IColumnExpression expression, Context context)
        {
            if (expression.Language == LanguageType.NCalc)
            {
                var exp = new NCalc.Expression(expression.Value);
                var factory = new ColumnIdentifierFactory();

                exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
                {
                    args.Result = name.StartsWith("@")
                        ? context.Variables[name.Substring(1, name.Length - 1)].GetValue()
                        : Execute(context, factory.Instantiate(name));
                };

                return exp.Evaluate();
            }
            else if (expression.Language == LanguageType.Native)
            {
                var parse = expression.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var variable = new ColumnIdentifierFactory().Instantiate(parse.ElementAt(0));
                var value = Execute(context, variable);

                foreach (var nativeFunction in parse.Skip(1))
                {
                    var factory = new NativeTransformationFactory(ServiceLocator, context);
                    var transformation = factory.Instantiate(nativeFunction);
                    value = transformation.Evaluate(value);
                }

                return value;
            }
            else
                throw new ArgumentOutOfRangeException($"The language {expression.Language} is not supported during the evaluation of an expression.");
        }

        //private class TransformationInfo : ITransformationInfo
        //{
        //    public ColumnType OriginalType { get; set; }
        //    public LanguageType Language { get; set; }
        //    public string Code { get; set; }
        //}
    }
}
