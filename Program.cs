using System;
using System.Collections.Generic;

namespace Compiler
{
    class Program
    {

        static List<Production> defineGrammar()
        {
            return new List<Production>() { Grammars.Productions.Expr, Grammars.Productions.ExprTail, Grammars.Productions.Term, Grammars.Productions.TermTail, Grammars.Productions.Factor };
        }
        static void Main(string[] args)
        {
            var pros = defineGrammar();
            foreach (var p in pros)
            {
                Console.WriteLine(p.ToString());
            }

            var parser = new LL1Parser(pros);
            var str = "123 * 234 + 345 / (678 - (90 * (123 + 234)))";
            var tokens = Lexer.Tokenizer(str);
            parser.Parse(tokens, str.Length);
            Console.ReadLine();

        }
    }
}
