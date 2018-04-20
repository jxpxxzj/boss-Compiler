using System;
using System.Collections.Generic;

namespace Compiler
{
    public class Grammar
    {
        public List<Production> Productions { get; set; } = new List<Production>();
        private bool[] used = new bool[500];
        private Dictionary<NonTerminal, int> vndic = new Dictionary<NonTerminal, int>();

        public Grammar(List<Production> productions)
        {
            Productions = productions;
            for (var i = 0; i < Productions.Count; i++)
            {
                vndic[Productions[i].Left] = i;
            }
            GenerateFirst();
            GenerateFollow();
        }

        public void GenerateFirst()
        {
            getFirst();
            foreach (var f in Productions)
            {
                if (f.Left is NonTerminal nont)
                {
                    Console.Write("First of {0}: [ ", f.Left.Name);
                    foreach (var i in nont.First)
                    {
                        Console.Write(i.Name + " ");
                    }
                    Console.WriteLine("]");
                }
            }
        }
        public void GenerateFollow()
        {
            getFollow();
            foreach (var f in Productions)
            {
                if (f.Left is NonTerminal nont)
                {
                    Console.Write("Follow of {0}: [ ", f.Left.Name);
                    foreach (var i in nont.Follow)
                    {
                        Console.Write(i.Name + " ");
                    }
                    Console.WriteLine("]");
                }
            }
        }

        private void calcFirst(int x)
        {
            if (used[x])
            {
                return;
            }
            used[x] = true;

            var left = Productions[x].Left;
            var right = Productions[x].Right;
            foreach (var expression in right)
            {

                for (int i = 0; i < expression.Count; i++)
                {
                    if (expression[i] is Terminal term)
                    {
                        left.First.Add(term);
                        break;
                    }
                    if (expression[i] is NonTerminal nonTerm)
                    {
                        int y = vndic[nonTerm];
                        calcFirst(y);

                        var temp = nonTerm.First;
                        bool flag = true;
                        foreach (var f in temp)
                        {
                            if (f == Terminal.Epsilon)
                            {
                                flag = false;
                            }
                            left.First.Add(f);
                        }
                        if (flag)
                        {
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        private void getFirst()
        {
            for (var i = 0; i < Productions.Count; i++)
            {
                calcFirst(i);
            }

        }
        private void getFollow()
        {
            Productions[0].Left.Follow.Add(Tokens.LexEnd);
            while (true)
            {
                bool loop = false;
                for (var i = 0; i < Productions.Count; i++)
                {
                    var left = Productions[i].Left;
                    var right = Productions[i].Right;
                    foreach (var proe in right)
                    {
                        var flag = true;
                        for (var j = proe.Count - 1; j >= 0; j--)
                        {
                            if (proe[j] is NonTerminal nontJ)
                            {
                                int x = vndic[nontJ];
                                if (flag)
                                {
                                    int before = nontJ.Follow.Count;
                                    nontJ.Follow.UnionWith(left.Follow);
                                    if (!Productions[x].HasEpsilonExpression)
                                    {
                                        flag = false;
                                    }
                                    int after = nontJ.Follow.Count;
                                    if (after > before)
                                    {
                                        loop = true;
                                    }
                                }
                                for (int k = j + 1; k < proe.Count; k++)
                                {
                                    if (proe[k] is NonTerminal nontK)
                                    {
                                        var from = nontK.First;
                                        var to = nontJ.Follow;
                                        int before = to.Count;
                                        foreach (var it1 in from)
                                        {
                                            if (it1 != Terminal.Epsilon)
                                            {
                                                to.Add(it1);
                                            }
                                        }
                                        int after = to.Count;
                                        if (after > before)
                                        {
                                            loop = true;
                                        }
                                        if (!Productions[vndic[nontK]].HasEpsilonExpression)
                                        {
                                            break;
                                        }
                                    }
                                    else if (proe[k] is Terminal termK)
                                    {
                                        int before = nontJ.Follow.Count;
                                        nontJ.Follow.Add(termK);
                                        int after = nontJ.Follow.Count;
                                        if (after > before)
                                        {
                                            loop = true;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }
                if (!loop)
                {
                    break;
                }
            }
        }
    }
}
