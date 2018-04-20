using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    public class ProductionExpression : List<AbstractTerminal>
    {
        public Production Parent { get; set; }
        public static ProductionExpression operator +(string str, ProductionExpression expr)
        {
            return (Terminal)str + expr;
        }

        public static ProductionExpression operator +(ProductionExpression expr, string str)
        {
            return expr + (Terminal)str;
        }

        public static List<ProductionExpression> operator |(ProductionExpression expr1, ProductionExpression expr2)
        {
            return new List<ProductionExpression>() { expr1, expr2 };
        }

        public static List<ProductionExpression> operator |(List<ProductionExpression> list, ProductionExpression expr)
        {
            list.Add(expr);
            return list;
        }

        public static implicit operator ProductionExpression(AbstractTerminal term) => new ProductionExpression { term };

        public override string ToString()
        {
            return ToString(false);
        }
        public string ToString(bool showParent)
        {
            var builder = new StringBuilder();
            if (showParent)
            {
                builder.Append(Parent.Name);
                builder.Append(" -> ");
            }
            foreach (var f in this)
            {
                builder.Append(f.Name);
                builder.Append(" ");
            }
            return builder.ToString().Trim();
        }

        public bool IsEpsilonExpression => Count == 1 && this[0] is Terminal term && term == Terminal.Epsilon;
    }
}
