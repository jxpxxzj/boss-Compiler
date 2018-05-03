using Compiler.Lexical;
using System;
using System.Collections.Generic;

namespace Compiler.Definition
{
    public class Terminal : AbstractTerminal, IEquatable<Terminal>
    {
        public Tokens Type;

        public static Terminal Epsilon = new Terminal("Epsilon", Tokens.Epsilon);
        public static Terminal LexEnd = new Terminal("LexEnd", Tokens.LexEnd);

        public new static IEqualityComparer<Terminal> Comparer => new TerminalComparer();

        public override HashSet<Terminal> First
        {
            get => new HashSet<Terminal> { this };
            set => throw new NotImplementedException();
        }

        public override HashSet<Terminal> Follow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Terminal(string name, Tokens token = Tokens.Undefined)
        {
            Name = name;
            Type = token;
        }

        public static bool operator ==(Terminal terminal1, Terminal terminal2)
        {
            return Comparer.Equals(terminal1, terminal2);
        }

        public static bool operator ==(AbstractTerminal abstractTerminal, Terminal terminal)
        {
            if (abstractTerminal is Terminal t)
            {
                return t == terminal;
            }
            return false;
        }
        public static bool operator !=(AbstractTerminal abstractTerminal, Terminal terminal)
        {
            return !(abstractTerminal == terminal);
        }

        public static bool operator !=(Terminal terminal1, Terminal terminal2)
        {
            return !(terminal1 == terminal2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Terminal);
        }

        public bool Equals(Terminal other)
        {
            return Comparer.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        private class TerminalComparer : IEqualityComparer<Terminal>
        {
            public bool Equals(Terminal x, Terminal y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x is null || y is null)
                {
                    return false;
                }

                if (x.Type == Tokens.Undefined && y.Type == Tokens.Undefined)
                {
                    return x.Name == y.Name;
                }

                return x.Type == y.Type;
            }

            public int GetHashCode(Terminal obj)
            {
                return 2049151605 + obj.Type.GetHashCode();
            }
        }
    }
}
