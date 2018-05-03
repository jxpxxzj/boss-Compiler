using Compiler.Definition;
using Compiler.Lexical;
using System;
using System.Collections.Generic;

namespace Compiler.Syntactic
{
    public abstract class AbstractParser
    {
        public Grammar Grammar { get; protected set; }

        public AbstractParser(Grammar grammar)
        {
            if (grammar.Productions.Count == 0)
            {
                throw new Exception("Grammar has no production.");
            }

            if (grammar.Productions[0].Right.Count == 0)
            {
                throw new Exception("Begin production has no right part.");
            }

            Grammar = grammar;
        }
        public AbstractParser(List<Production> grammar) : this(new Grammar(grammar))
        {

        }

        public abstract bool Parse(List<Token> input, int textLength = -1);

        public virtual void ErrorHandler(int steps, Token token)
        {
            Console.WriteLine("{0}\tSyntax Error, Unexpected token: {1} at pos: {2}", steps, token.Text, token.StartLocation);
        }
    }
}
