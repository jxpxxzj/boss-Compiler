using Compiler.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Syntactic.LR
{
    public class Closure : IEquatable<Closure>
    {
        public List<ProductionExpression> Element { get; set; } = new List<ProductionExpression>();
        public ProductionExpression this[int index] { get => Element[index]; }

        public static IEqualityComparer<Closure> Comparer => new ClosureComparer();

        public int Count => Element.Count;

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var elem in Element)
            {
                sb.AppendLine(elem.ToString(true));
            }
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Closure);
        }

        public bool Equals(Closure other)
        {
            return Comparer.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        public static bool operator ==(Closure closure1, Closure closure2)
        {
            return Comparer.Equals(closure1, closure2);
        }

        public static bool operator !=(Closure closure1, Closure closure2)
        {
            return !(closure1 == closure2);
        }

        private class ClosureComparer : IEqualityComparer<Closure>
        {
            public bool Equals(Closure x, Closure y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x is null || y is null)
                {
                    return false;
                }

                if (x.Count != y.Count)
                {
                    return false;
                }
                for (var i = 0; i < x.Count; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(Closure obj)
            {
                return 1951047055 + EqualityComparer<List<ProductionExpression>>.Default.GetHashCode(obj.Element);
            }
        }
    }
}
