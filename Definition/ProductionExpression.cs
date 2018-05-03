using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Definition
{
    public class ProductionExpression : List<AbstractTerminal>, IEquatable<ProductionExpression>
    {
        public Production Parent { get; set; }

        public static IEqualityComparer<ProductionExpression> Comparer => new ProductionExpressionComparer();

        public static ProductionExpression operator +(string str, ProductionExpression expr)
        {
            var builder = new ProductionExpressionBuilder(str);
            builder.Append(expr);
            return builder.ToExpression();
        }

        public static ProductionExpression operator +(ProductionExpression expr, string str)
        {
            var builder = new ProductionExpressionBuilder(expr);
            builder.Append(str);
            return builder.ToExpression();
        }

        public static ProductionExpression operator +(ProductionExpression expr1, ProductionExpression expr2)
        {
            var builder = new ProductionExpressionBuilder(expr1);
            builder.Append(expr2);
            return builder.ToExpression();
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

        public static bool operator ==(ProductionExpression expression1, ProductionExpression expression2)
        {
            return Comparer.Equals(expression1, expression2);
        }

        public static bool operator !=(ProductionExpression expression1, ProductionExpression expression2)
        {
            return !(expression1 == expression2);
        }

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

        public ProductionExpression SubExpr(int start)
        {
            return SubExpr(start, Count);
        }
        public ProductionExpression SubExpr(int start, int length)
        {
            int cnt = 0;
            var pe = new ProductionExpression
            {
                Parent = Parent
            };
            for (var i = start; i < Count; i++)
            {
                pe.Add(this[i]);
                cnt++;
                if (cnt == length)
                {
                    break;
                }
            }
            return pe;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProductionExpression);
        }

        public bool Equals(ProductionExpression other)
        {
            return Comparer.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        public ProductionExpression Clone()
        {
            var pe = new ProductionExpression()
            {
                Parent = Parent
            };
            ForEach(t => pe.Add(t));
            return pe;
        }

        private class ProductionExpressionComparer : IEqualityComparer<ProductionExpression>
        {
            public bool Equals(ProductionExpression expression1, ProductionExpression expression2)
            {
                if (ReferenceEquals(expression1, expression2))
                {
                    return true;
                }
                if (expression1 is null || expression2 is null)
                {
                    return false;
                }
                if (expression1.Count != expression2.Count) { return false; }
                if (expression1.Parent.Left != expression2.Parent.Left) { return false; }
                for (int i = 0; i < expression1.Count; i++)
                {
                    //TODO: considering cast to Terminal or NonTerminal to avoid duplicate Terminals?
                    if (expression1[i] != expression2[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(ProductionExpression obj)
            {
                //TODO: improve perfomance by cache result?
                int hashCode = -1801665627;
                foreach (var elem in obj)
                {
                    hashCode += elem.GetHashCode();
                }
                return hashCode;
            }
        }
    }
}
