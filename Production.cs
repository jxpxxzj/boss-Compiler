using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    public class Production
    {
        public string Name { get; set; }
        public NonTerminal Left { get; set; }
        public List<ProductionExpression> Right { get; protected set; } = new List<ProductionExpression>();

        public Production(string name, NonTerminal left)
        {
            Name = name;
            Left = left;
        }

        public Production Add(AbstractTerminal term)
        {
            var expr = new ProductionExpression() { term };
            expr.Parent = this;

            Right.Add(expr);
            return this;
        }

        public Production Add(ProductionExpression expr)
        {
            Right.Add(expr);
            expr.Parent = this;
            return this;
        }

        public Production Add(IEnumerable<ProductionExpression> exprList)
        {
            foreach (var f in exprList)
            {
                Add(f);
            }
            return this;
        }

        public bool HasEpsilonExpression => Right.FindIndex(t => t.IsEpsilonExpression) != -1;

        public static implicit operator Production(NonTerminal term)
        {
            return new Production(term.Name, term);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Name);
            builder.Append("\r\n");
            for (var i = 0; i < Right.Count; i++)
            {
                builder.Append("\t");
                if (i == 0)
                {
                    builder.Append(": ");
                }
                if (i > 0)
                {
                    builder.Append("| ");
                }
                var str = Right[i].ToString();
                builder.Append(str);
                builder.Append("\r\n");
            }
            return builder.ToString().Trim();
        }

    }
}
