﻿using NBi.Core.Evaluate;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class Context
    {

        public IEnumerable<IColumnAlias> Aliases { get; } = [];
        public IEnumerable<IColumnExpression> Expressions { get; } = [];

        public IDictionary<string, IVariable> Variables { get; } = new Dictionary<string, IVariable>();
        public IResultRow? CurrentRow { get; private set; }

        public static Context None { get; } = new Context();

        private Context() { }

        public Context(IDictionary<string, IVariable> variables)
            => Variables = variables;

        public Context(IDictionary<string, IVariable> variables, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(variables) => (Aliases, Expressions) = (aliases, expressions);

        public void Switch(IResultRow currentRow)
            => CurrentRow = currentRow;
    }
}
