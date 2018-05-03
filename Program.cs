using Compiler.Definition;
using Compiler.Lexical;
using Compiler.Syntactic;
using System;
using System.Collections.Generic;

namespace Compiler
{
    class Program
    {
        static List<Production> defineGrammar()
        {
            return new List<Production>() { LL1TestGrammars.Productions.Expr, LL1TestGrammars.Productions.ExprTail, LL1TestGrammars.Productions.Term, LL1TestGrammars.Productions.TermTail, LL1TestGrammars.Productions.Factor };
        }
        static void Main(string[] args)
        {
            //var pros = defineGrammar();
            //foreach (var p in pros)
            //{
            //    Console.WriteLine(p.ToString());
            //}

            var str = "123 + 456 * 789 - (222 / 333 + 444 * 555 * (666 - 777 * 888))";
            // var str = "+--*/(";
            //var str = "(123 * 456) + 789";
            var tokens = Lexer.Lex(str);

            var pros = new List<Production>
            {
                LRTestGrammar.Productions.S, LRTestGrammar.Productions.E, LRTestGrammar.Productions.A, LRTestGrammar.Productions.B
            };

            var pros2 = new List<Production>
            {
                ExpressionGrammar.Productions.S, ExpressionGrammar.Productions.E, ExpressionGrammar.Productions.T, ExpressionGrammar.Productions.F
            };

            var pros3 = new List<Production>
            {
                TextBookTestGrammar.Productions.S, TextBookTestGrammar.Productions.E, TextBookTestGrammar.Productions.A, TextBookTestGrammar.Productions.B
            };

            var parser = new SLR1Parser(pros2);
            parser.Parse(tokens, str.Length);

            Console.ReadLine();

        }
    }
}
