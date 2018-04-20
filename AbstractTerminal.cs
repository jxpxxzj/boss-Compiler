using System.Collections.Generic;

namespace Compiler
{
    public abstract class AbstractTerminal
    {
        public string Name { get; set; }
        public abstract HashSet<Terminal> First { get; set; }
        public abstract HashSet<Terminal> Follow { get; set; }

        public static ProductionExpression operator +(ProductionExpression expr, AbstractTerminal self)
        {
            expr.Add(self);
            return expr;
        }

        public static ProductionExpression operator +(AbstractTerminal old, AbstractTerminal self)
        {
            var expr = new ProductionExpression
            {
                old,
                self
            };
            return expr;
        }

        public static ProductionExpression operator +(AbstractTerminal self, ProductionExpression expr)
        {
            expr.Insert(0, self);
            return expr;
        }

        public static ProductionExpression operator +(string old, AbstractTerminal self)
        {
            return (Terminal)old + self;
        }

        public static ProductionExpression operator +(AbstractTerminal old, string self)
        {
            return old + (Terminal)self;
        }

        public static List<ProductionExpression> operator |(List<ProductionExpression> list, AbstractTerminal term)
        {
            list.Add(term);
            return list;
        }

        public static implicit operator AbstractTerminal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Terminal.Epsilon;
            }
            var type = Token.GenerateType(str);
            return new Terminal(type.ToString(), type);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
