using System;
using System.Collections.Generic;

namespace Compiler.Definition
{
    public abstract class AbstractTerminal : IEquatable<AbstractTerminal>
    {
        public string Name { get; set; }
        public abstract HashSet<Terminal> First { get; set; }
        public abstract HashSet<Terminal> Follow { get; set; }

        public static IEqualityComparer<AbstractTerminal> Comparer => new AbstractTerminalComparer();

        public static ProductionExpression operator +(ProductionExpression expr, AbstractTerminal self)
        {
            var builder = new ProductionExpressionBuilder(expr);
            builder.Append(self);
            return builder.ToExpression();
        }

        public static ProductionExpression operator +(AbstractTerminal old, AbstractTerminal self)
        {
            var builder = new ProductionExpressionBuilder(old);
            builder.Append(self);
            return builder.ToExpression();
        }

        public static ProductionExpression operator +(AbstractTerminal self, ProductionExpression expr)
        {
            var builder = new ProductionExpressionBuilder(self);
            builder.Append(expr);
            return builder.ToExpression();
        }

        public static ProductionExpression operator +(string old, AbstractTerminal self)
        {
            var builder = new ProductionExpressionBuilder(old);
            builder.Append(self);
            return builder.ToExpression();
        }

        public static ProductionExpression operator +(AbstractTerminal old, string self)
        {
            var builder = new ProductionExpressionBuilder(old);
            builder.Append(self);
            return builder.ToExpression();
        }

        public static List<ProductionExpression> operator |(List<ProductionExpression> list, AbstractTerminal term)
        {
            var builder = new ProductionExpressionBuilder(term);
            list.Add(builder.ToExpression());
            return list;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AbstractTerminal);
        }

        public bool Equals(AbstractTerminal other)
        {
            return Comparer.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        public static bool operator ==(AbstractTerminal at1, AbstractTerminal at2)
        {
            return Comparer.Equals(at1, at2);
        }

        public static bool operator !=(AbstractTerminal at1, AbstractTerminal at2)
        {
            return !(at1 == at2);
        }

        private class AbstractTerminalComparer : IEqualityComparer<AbstractTerminal>
        {
            public bool Equals(AbstractTerminal x, AbstractTerminal y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x is null || y is null)
                {
                    return false;
                }

                if (x is Terminal t1 && y is Terminal t2)
                {
                    return t1 == t2;
                }

                return false;
            }

            public int GetHashCode(AbstractTerminal obj)
            {
                return 539060726 + EqualityComparer<string>.Default.GetHashCode(obj.Name);
            }
        }
    }
}
