namespace Compiler
{
    /*
    Infix Expression:
    Expr -> Expr + Term
          | Expr - Term
          | Term
    Term -> Term * Factor
          | Term / Factor
          | Factor
    Factor -> (Expr)
           | num

    LL(1) Grammar:
    Expr -> Term ExprTail
    ExprTail -> + Term ExprTail
              | - Term ExprTail
              | null
    Term -> Factor TermTail
    TermTail -> * Factor TermTail
              | / Factor TermTail
              | null
    Factor -> (Expr)
            | num
    */
    public static class Grammars
    {
        public static class NonTerminals
        {
            public static NonTerminal Expr;
            public static NonTerminal ExprTail;
            public static NonTerminal Term;
            public static NonTerminal TermTail;
            public static NonTerminal Factor;

            static NonTerminals()
            {
                Expr = new NonTerminal("Expr");
                ExprTail = new NonTerminal("ExprTail");
                Term = new NonTerminal("Term");
                TermTail = new NonTerminal("TermTail");
                Factor = new NonTerminal("Factor");
            }
        }

        public static class Terminals
        {
            public static Terminal Number = Tokens.Number;
        }

        public static class Productions
        {
            public static Production Expr;
            public static Production ExprTail;
            public static Production Term;
            public static Production TermTail;
            public static Production Factor;

            static Productions()
            {
                Expr = NonTerminals.Expr;
                ExprTail = NonTerminals.ExprTail;
                Term = NonTerminals.Term;
                TermTail = NonTerminals.TermTail;
                Factor = NonTerminals.Factor;

                Expr.Add(NonTerminals.Term + NonTerminals.ExprTail);

                ExprTail.Add( ("+" + NonTerminals.Term + NonTerminals.ExprTail)
                            | ("-" + NonTerminals.Term + NonTerminals.ExprTail)
                            | (Terminal.Epsilon)
                            );

                Term.Add(NonTerminals.Factor + NonTerminals.TermTail);

                TermTail.Add( ("*" + NonTerminals.Factor + NonTerminals.TermTail)
                            | ("/" + NonTerminals.Factor + NonTerminals.TermTail)
                            | (Terminal.Epsilon));

                Factor.Add( ("(" + NonTerminals.Expr + ")") 
                          | (Terminals.Number)
                          );
            }
        }
    }
}
