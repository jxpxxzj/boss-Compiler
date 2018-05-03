using System.Collections.Generic;

namespace Compiler.Definition
{
    public class NonTerminal : AbstractTerminal
    {
        private HashSet<Terminal> internalFirst = new HashSet<Terminal>();
        private HashSet<Terminal> internalFollow = new HashSet<Terminal>();

        public override HashSet<Terminal> First { get => internalFirst; set => internalFirst = value; }
        public override HashSet<Terminal> Follow { get => internalFollow; set => internalFollow = value; }

        public NonTerminal(string name)
        {
            Name = name;
        }

        public static implicit operator Production(NonTerminal nont) => TypeConventer.ToProduction(nont);
    }
}