using System;
using System.Collections.Generic;

namespace Compiler
{
    public class Terminal : AbstractTerminal, IEquatable<Terminal>
    {
        public Tokens Type;

        public static Terminal Epsilon = new Terminal("Epsilon", Tokens.Epsilon);
        public static Terminal LexEnd = new Terminal("LexEnd", Tokens.LexEnd);
        public static TerminalComparer Comparer { get; } = new TerminalComparer();

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

        public static implicit operator Terminal(string t)
        {
            if (string.IsNullOrEmpty(t))
            {
                return Epsilon;
            }
            var type = Token.GenerateType(t);
            return new Terminal(type.ToString(), type);
        }

        public static implicit operator Terminal(Token t)
        {
            if (t.Type == Tokens.LexEnd)
            {
                return new Terminal(t.Type.ToString(), t.Type);
            }
            if (t == null || string.IsNullOrEmpty(t.Text) || t.Type == Tokens.Epsilon)
            {
                return Epsilon;
            }
            return new Terminal(t.Type.ToString(), t.Type);
        }

        public static implicit operator Terminal(Tokens t)
        {
            return new Terminal(t.ToString(), t);
        }

        public static bool operator ==(Terminal terminal1, Terminal terminal2)
        {
            return EqualityComparer<Terminal>.Default.Equals(terminal1, terminal2);
        }

        public static bool operator !=(Terminal terminal1, Terminal terminal2)
        {
            return !(terminal1 == terminal2);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Terminal)) return false;
            return Equals((Terminal)obj);
        }

        public bool Equals(Terminal other)
        {
            return other != null &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return 2049151605 + Type.GetHashCode();
        }

        public class TerminalComparer : IEqualityComparer<Terminal>
        {
            public bool Equals(Terminal x, Terminal y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(Terminal obj)
            {
                return obj.Type.GetHashCode();
            }
        }
    }
}
