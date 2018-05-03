using System;

namespace Compiler.Definition
{
    public class ProductionExpressionBuilder
    {
        private ProductionExpression expression;

        public ProductionExpressionBuilder(string str)
        {
            expression = new ProductionExpression() { TypeConventer.ToTerminal(str) };
        }

        public ProductionExpressionBuilder(AbstractTerminal abs)
        {
            expression = new ProductionExpression() { abs };
        }

        public ProductionExpressionBuilder(Terminal term)
        {
            expression = new ProductionExpression() { term };
        }

        public ProductionExpressionBuilder(NonTerminal nont)
        {
            expression = new ProductionExpression { nont };
        }

        public ProductionExpressionBuilder(ProductionExpression expr)
        {
            if (expr != null)
            {
                expression = expr;
            }
            else
            {
                throw new NullReferenceException("Expression can't be null.");
            }
        }

        public void Append(string str)
        {
            expression.Add(TypeConventer.ToTerminal(str));
        }

        public void Append(NonTerminal nont)
        {
            expression.Add(nont);
        }

        public void Append(Terminal term)
        {
            expression.Add(term);
        }

        public void Append(AbstractTerminal abs)
        {
            expression.Add(abs);
        }

        public void Append(ProductionExpression expr)
        {
            expr.ForEach(t => expression.Add(t));
        }

        public ProductionExpression ToExpression()
        {
            return expression.Clone();
        }

    }
}
