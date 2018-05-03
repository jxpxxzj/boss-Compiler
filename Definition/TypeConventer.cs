using Compiler.Lexical;

namespace Compiler.Definition
{
    public static class TypeConventer
    {
        public static Terminal ToTerminal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Terminal.Epsilon;
            }
            var type = Token.GenerateType(str);
            return new Terminal(type.ToString(), type);
        }

        public static Terminal ToTerminal(Token t)
        {
            if (t.Type == Tokens.LexEnd)
            {
                return new Terminal(t.Type.ToString(), t.Type);
            }
            if (t == null || string.IsNullOrEmpty(t.Text) || t.Type == Tokens.Epsilon)
            {
                return Terminal.Epsilon;
            }
            return new Terminal(t.Type.ToString(), t.Type);
        }

        public static Terminal ToTerminal(Tokens t)
        {
            return new Terminal(t.ToString(), t);
        }

        public static AbstractTerminal ToAbstractTerminal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Terminal.Epsilon;
            }
            var type = Token.GenerateType(str);
            return new Terminal(type.ToString(), type);
        }

        public static Production ToProduction(NonTerminal nont)
        {
            return new Production(nont.Name, nont);
        }
    }
}
