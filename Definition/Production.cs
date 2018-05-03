using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Definition
{
    public class Production : IEquatable<Production>
    {
        public string Name { get; set; }
        public NonTerminal Left { get; set; }
        public List<ProductionExpression> Right { get; protected set; } = new List<ProductionExpression>();

        public static IEqualityComparer<Production> Comparer => new ProductionComparer();

        public Production(NonTerminal left)
        {
            Name = left.Name;
            Left = left;
        }

        public Production(string name, NonTerminal left)
        {
            Name = name;
            Left = left;
        }

        public Production Add(string str)
        {
            var builder = new ProductionExpressionBuilder(str);
            var expr = builder.ToExpression();
            Add(expr);
            return this;
        }

        public Production Add(AbstractTerminal term)
        {
            var expr = new ProductionExpression() { term };
            Add(expr);
            return this;
        }

        public Production Add(ProductionExpression expr)
        {
            expr.Parent = this;
            if (Right.Contains(expr))
            {
                throw new Exception("Duplicated expression.");
            }

            Right.Add(expr);
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

        public static bool operator ==(Production production1, Production production2)
        {
            return Comparer.Equals(production1, production2);
        }

        public static bool operator !=(Production production1, Production production2)
        {
            return !(production1 == production2);
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

        public List<Production> Expand()
        {
            var prod = new List<Production>();
            for (int i = 0; i < Right.Count; i++)
            {
                var p = new Production(Left.Name + "_" + i, Left);
                p.Add(Right[i]);
                prod.Add(p);
            }
            return prod;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Production);
        }

        public bool Equals(Production other)
        {
            return Comparer.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        private class ProductionComparer : IEqualityComparer<Production>
        {
            public bool Equals(Production production1, Production production2)
            {
                if (ReferenceEquals(production1, production2))
                {
                    return true;
                }
                if (production1 is null || production2 is null)
                {
                    return false;
                }
                if (production1.Right.Count != production2.Right.Count)
                {
                    return false;
                }
                if (production1.Left != production2.Left)
                {
                    return false;
                }
                for (int i = 0; i < production1.Right.Count; i++)
                {
                    if (!production2.Right.Contains(production1.Right[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(Production obj)
            {
                var hashCode = -1051820395;
                hashCode = hashCode * -1521134295 + EqualityComparer<NonTerminal>.Default.GetHashCode(obj.Left);
                hashCode = hashCode * -1521134295 + EqualityComparer<List<ProductionExpression>>.Default.GetHashCode(obj.Right);
                return hashCode;
            }
        }
    }
}
