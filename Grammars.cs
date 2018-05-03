using Compiler.Definition;
using Compiler.Lexical;

namespace Compiler
{
    public static class TextBookTestGrammar
    {
        public static class NonTerminals
        {
            public static NonTerminal S;
            public static NonTerminal E;
            public static NonTerminal A;
            public static NonTerminal B;

            static NonTerminals()
            {
                S = new NonTerminal("S");
                E = new NonTerminal("E");
                A = new NonTerminal("A");
                B = new NonTerminal("B");
            }
        }

        public static class Productions
        {
            public static Production S;
            public static Production E;
            public static Production A;
            public static Production B;

            static Productions()
            {
                S = NonTerminals.S;
                E = NonTerminals.E;
                A = NonTerminals.A;
                B = NonTerminals.B;

                S.Add(NonTerminals.E);
                E.Add("+" + NonTerminals.A)
                    .Add("-" + NonTerminals.B);
                A.Add("*" + NonTerminals.A)
                    .Add("/");
                B.Add("*" + NonTerminals.B)
                    .Add("/");
            }
        }
    }
    public static class LeftRecursiveTestGrammar
    {
        public static class NonTerminals
        {
            public static NonTerminal E;
            public static NonTerminal T;
            public static NonTerminal F;
            public static NonTerminal S;
            static NonTerminals()
            {
                E = new NonTerminal("E");
                T = new NonTerminal("T");
                F = new NonTerminal("F");
                S = new NonTerminal("S");
            }
        }
        public static class Terminals
        {
            public static Terminal Number = TypeConventer.ToTerminal(Tokens.Number);
        }
        public static class Productions
        {
            public static Production E;
            public static Production T;
            public static Production F;
            public static Production S;

            static Productions()
            {
                S = NonTerminals.S;
                E = NonTerminals.E;
                T = NonTerminals.T;
                F = NonTerminals.F;

                S.Add(NonTerminals.E);

                E.Add(NonTerminals.E + "+" + NonTerminals.T)
                    .Add(NonTerminals.T);

                T.Add(NonTerminals.T + "*" + NonTerminals.F)
                    .Add(NonTerminals.F);

                F.Add("(" + NonTerminals.E + ")")
                 .Add(Terminals.Number);
            }
        }
    }
    public static class LRTestGrammar
    {
        public static class NonTerminals
        {
            public static NonTerminal S;
            public static NonTerminal E;
            public static NonTerminal A;
            public static NonTerminal B;

            static NonTerminals()
            {
                S = new NonTerminal("S");
                E = new NonTerminal("E");
                A = new NonTerminal("A");
                B = new NonTerminal("B");
            }
        }
        public static class Terminals
        {

        }
        public static class Productions
        {
            public static Production S;
            public static Production E;
            public static Production A;
            public static Production B;

            static Productions()
            {
                S = NonTerminals.S;
                E = NonTerminals.E;
                A = NonTerminals.A;
                B = NonTerminals.B;

                S.Add(NonTerminals.E);
                E.Add("+" + NonTerminals.A + "*" + NonTerminals.B + "(");
                A.Add(NonTerminals.A + "-").Add("-");
                B.Add("/");
            }
        }
    }

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
    public static class LL1TestGrammars
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
            public static Terminal Number = TypeConventer.ToTerminal(Tokens.Number);
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

                ExprTail.Add(("+" + NonTerminals.Term + NonTerminals.ExprTail)
                            | ("-" + NonTerminals.Term + NonTerminals.ExprTail)
                            | (Terminal.Epsilon)
                            );

                Term.Add(NonTerminals.Factor + NonTerminals.TermTail);

                TermTail.Add(("*" + NonTerminals.Factor + NonTerminals.TermTail)
                            | ("/" + NonTerminals.Factor + NonTerminals.TermTail)
                            | (Terminal.Epsilon)
                            );

                Factor.Add("(" + NonTerminals.Expr + ")")
                      .Add(Terminals.Number);
            }
        }
    }
}
