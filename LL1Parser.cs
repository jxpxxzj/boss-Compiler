using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public class LL1Parser
    {
        public Grammar Grammar { get; protected set; }

        private Dictionary<NonTerminal, Dictionary<Terminal, ProductionExpression>> predictTable = new Dictionary<NonTerminal, Dictionary<Terminal, ProductionExpression>>();
        
        public LL1Parser(Grammar grammar)
        {
            Grammar = grammar;
            generateTable();
        }

        public LL1Parser(List<Production> productions)
        {
            Grammar = new Grammar(productions);
            generateTable();
        }
        
        private bool checkFirst(ProductionExpression expression, AbstractTerminal term)
        {
            for (int i = 0; i < expression.Count; i++)
            {
                bool hasEmpty = false;
                if (expression[i] is Terminal terminal)
                {
                    return terminal == (Terminal)term;
                }
                else if (expression[i] is NonTerminal nonT)
                {
                    var dic = nonT.First;
                    foreach (var its in dic)
                    {
                        if (its == Terminal.Epsilon)
                        {
                            hasEmpty = true;
                        }
                        if (its == (Terminal)term)
                        {
                            return true;
                        }
                    }
                    if (!hasEmpty)
                    {
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }
            return false;
        }

        private List<Terminal> getTable()
        {
            var dict = new Dictionary<Terminal, ProductionExpression>();
            var letters = new List<Terminal>();
            foreach (var e1 in Grammar.Productions)
            {
                var right = e1.Right;
                foreach (var e2 in right)
                {
                    if (e2.IsEpsilonExpression)
                    {
                        continue;
                    }
                    foreach (var term in e2)
                    {
                        if (term is Terminal letter)
                        {
                            letters.Add(letter);
                        }
                    }
                }
            }
            letters.Add(Terminal.LexEnd);
            for (int i = 0; i < Grammar.Productions.Count; i++)
            {
                dict = new Dictionary<Terminal, ProductionExpression>(Terminal.Comparer);
                var left = Grammar.Productions[i].Left;
                var right = Grammar.Productions[i].Right;
                foreach (var it in right)
                {
                    for (int j = 0; j < letters.Count; j++)
                    {
                        if (checkFirst(it, letters[j]))
                        {
                            dict[letters[j]] = it;
                        }
                        if (checkFirst(it, Terminal.Epsilon))
                        {
                            foreach (var b in left.Follow)
                            {
                                dict[b] = it;
                            }
                        }

                    }
                }
                predictTable[left] = dict;
            }
            return letters;
        }

        private void generateTable()
        {
            getTable();
            foreach (var t1 in predictTable)
            {
                Console.WriteLine("{0}:", t1.Key);
                foreach (var t2 in t1.Value)
                {
                    Console.WriteLine("\t{0}:\t{1}", t2.Key, t2.Value.ToString(true));
                }
            }
        }

        private void printStep(int step, int pointer, Stack<AbstractTerminal> stack, List<Token> input, ProductionExpression expr = null, AbstractTerminal currentTop = null)
        {
            var arrStack = stack.ToList();
            arrStack.Reverse();
            if (currentTop != null)
            {
                arrStack.Add(currentTop);
            }

            var sb = new StringBuilder();
            foreach (var item in arrStack)
            {
                sb.Append("[");
                sb.Append(item.Name);
                sb.Append("]");
            }
            var stackStr = sb.ToString();

            var arrInput = input.Skip(pointer);
            sb = new StringBuilder();
            foreach (var item in arrInput)
            {
                sb.Append("[");
                sb.Append(item.Text);
                sb.Append("]");
            }
            var inputStr = sb.ToString();

            var str = string.Format("{0}\t{1}\t{2}", step, stackStr, inputStr);
            if (expr != null)
            {
                str += ("\t" + expr.ToString(true));
            }
            Console.WriteLine(str);
        }

        public bool Parse(List<Token> input, int textLength = -1)
        {
            input.Add(new Token() { Type = Tokens.LexEnd, Text = "#" ,EndLocation = textLength });
            var stack = new Stack<AbstractTerminal>();

            int pointer = 0;
            var token = input[pointer];

            stack.Push(Terminal.LexEnd);
            stack.Push(Grammar.Productions[0].Left);
            var flag = true;
            int step = 0;
            printStep(step, pointer, stack, input, null, null);
            while (flag)
            {
                step++;
                var x = stack.Pop();
                if (x is Terminal termX && termX == Terminal.LexEnd)
                {
                    if (((Terminal)x).Type == token.Type)
                    {
                        flag = false;
                        Console.WriteLine("Accept");
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (x is Terminal term)
                {
                    if (term.Type == token.Type)
                    {
                        pointer++;
                        token = input[pointer];
                        printStep(step, pointer, stack, input, null, x);
                    }
                }
                else if (x is NonTerminal nont)
                {
                    try
                    {
                        var dict = predictTable[nont];
                        if (dict.ContainsKey(token))
                        {
                            var expr = dict[token];
                            if (expr.IsEpsilonExpression)
                            {
                                printStep(step, pointer, stack, input, expr, null);
                                continue;
                            }
                            for (var i = expr.Count - 1; i >= 0; i--)
                            {
                                stack.Push(expr[i]);
                            }
                            printStep(step, pointer, stack, input, expr, null);
                        }
                        else
                        {
                            Console.WriteLine("{0}\tSyntax Error, Unexpected token: {1} at pos:{2}", step, token.Text, token.StartLocation);
                            return false;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return true;
        }
    }
}
