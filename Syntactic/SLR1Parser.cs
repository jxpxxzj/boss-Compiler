using Compiler.Definition;
using Compiler.Syntactic.LR;
using System.Collections.Generic;

namespace Compiler.Syntactic
{
    public class SLR1Parser : LR0Parser
    {
        public SLR1Parser(Grammar grammar) : base(grammar)
        {
        }

        public SLR1Parser(List<Production> production) : base(production)
        {
        }

        protected override void makeTable()
        {
            for (int i = 0; i < Collections.Count; i++)
            {
                for (int j = 0; j < V.Count; j++)
                {
                    var ch = V[j];
                    if (!(Go.ContainsKey(i) && Go[i].ContainsKey(ch)))
                    {
                        continue;
                    }
                    int x = Go[i][ch];
                    if (x == -1) continue;
                    if (ch is Terminal term)
                    {
                        if (!Action.ContainsKey(i))
                        {
                            Action.Add(i, new Dictionary<Terminal, Content>());
                        }

                        Action[i].Add(term, new Content(LRTableType.Shift, x));
                    }
                    else if (ch is NonTerminal nt)
                    {
                        if (!Goto.ContainsKey(i))
                        {
                            Goto.Add(i, new Dictionary<NonTerminal, int>());
                        }
                        Goto[i].Add(nt, x);
                    }
                }
            }

            for (int i = 0; i < Collections.Count; i++)
            {
                for (int j = 0; j < Collections[i].Count; j++)
                {
                    var tt = Collections[i][j];
                    if (tt[tt.Count - 1] == dotTerminal)
                    {
                        if (tt.Parent.Left == Grammar.BeginSymbol)
                        {
                            if (!Action.ContainsKey(i))
                            {
                                Action.Add(i, new Dictionary<Terminal, Content>());
                            }
                            Action[i].Add(Terminal.LexEnd, new Content(LRTableType.Accept, -1));
                        }
                        else
                        {
                            for (int k = 0; k < V.Count; k++)
                            {
                                var y = V[k];
                                if (y is Terminal term)
                                {
                                    if (!tt.Parent.Left.Follow.Contains(term))
                                    {
                                        continue;
                                    }

                                    if (!Action.ContainsKey(i))
                                    {
                                        Action.Add(i, new Dictionary<Terminal, Content>());
                                    }
                                    var noDot = tt.Clone();
                                    noDot.Remove(dotTerminal);
                                    var cont = new Content(LRTableType.Reduce, -1)
                                    {
                                        ReduceProductionExpression = prodDict[noDot]
                                    };
                                    Action[i].Add(term, cont);
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
