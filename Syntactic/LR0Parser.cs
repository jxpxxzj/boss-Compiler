using Compiler.Definition;
using Compiler.Lexical;
using Compiler.Syntactic.LR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Syntactic
{
    public class LR0Parser : AbstractParser
    {
        protected List<Closure> Collections { get; set; } = new List<Closure>();
        protected Dictionary<NonTerminal, List<int>> dic = new Dictionary<NonTerminal, List<int>>();
        protected List<AbstractTerminal> V = new List<AbstractTerminal>();
        protected Dictionary<int, Dictionary<AbstractTerminal, int>> Go = new Dictionary<int, Dictionary<AbstractTerminal, int>>();
         
        protected Dictionary<int, Dictionary<Terminal, Content>> Action = new Dictionary<int, Dictionary<Terminal, Content>>();
        protected Dictionary<int, Dictionary<NonTerminal, int>> Goto = new Dictionary<int, Dictionary<NonTerminal, int>>();
        protected Dictionary<ProductionExpression, Tuple<int, int>> prodDict = new Dictionary<ProductionExpression, Tuple<int, int>>(ProductionExpression.Comparer);
         
        protected List<Production> items = new List<Production>();

        public LR0Parser(Grammar grammar) : base(grammar)
        {
            if (grammar.Productions[0].Right.Count != 1)
            {
                throw new Exception("Grammar has more than one entrance.");
            }

            foreach (var pd in grammar.Productions)
            {
                dic.Add(pd.Left, new List<int>());
            }
            MakeItem();
            MakeSet();
            makeV();
            MakeGo();
            MakeTable();
        }

        public LR0Parser(List<Production> production) : this(new Grammar(production))
        {

        }

        protected Terminal dotTerminal = new Terminal(".", Tokens.DotTerminal);

        public void MakeItem()
        {
            makeItem();
            foreach (var i in items)
            {
                Console.WriteLine(i.ToString());
            }
        }

        public void MakeSet()
        {
            makeSet();
            for (var i = 0; i < Collections.Count; i++)
            {
                Console.WriteLine("Closure-I{0}:", i);
                Console.WriteLine(Collections[i].ToString());
            }
        }

        private void makeItem()
        {
            int current = 0;
            for (int i = 0; i < Grammar.Productions.Count; i++)
            {
                var prod = new Production(Grammar.Productions[i].Name, Grammar.Productions[i].Left);
                for (var j = 0; j < Grammar.Productions[i].Right.Count; j++)
                {
                    var temp = Grammar.Productions[i].Right[j];
                    for (int k = 0; k <= temp.Count; k++)
                    {
                        var pe = new ProductionExpression()
                        {
                            Parent = Grammar.Productions[i]
                        };
                        pe.AddRange(temp);
                        if (pe.IsEpsilonExpression)
                        {
                            pe.Clear();
                            pe.Add(dotTerminal);
                            prod.Add(pe);
                            dic[prod.Left].Add(current);
                            current++;
                            break;
                        }
                        else
                        {
                            pe.Insert(k, dotTerminal);
                        }
                        prod.Add(pe);
                        dic[prod.Left].Add(current);
                        current++;
                    }
                    prodDict.Add(temp, new Tuple<int, int>(i, j));
                }
                items.AddRange(prod.Expand());
            }
        }

        protected virtual void makeSet()
        {
            bool[] has = new bool[500];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Left == Grammar.BeginSymbol && ReferenceEquals(items[i].Right[0][0], dotTerminal))
                {
                    var temp = new Closure();
                    var str = items[i].Right[0];
                    var element = temp.Element;
                    element.Add(items[i].Right[0]);
                    int x = 0;
                    for (x = 0; x < str.Count; x++)
                    {
                        if (ReferenceEquals(str[x], dotTerminal))
                        {
                            break;
                        }
                    }

                    has = new bool[500];
                    has[i] = true;
                    if (x != str.Count - 1)
                    {
                        var q = new Queue<ProductionExpression>();
                        q.Enqueue(str.SubExpr(x + 1, 1));
                        while (!(q.Count == 0))
                        {
                            var u = q.Peek();
                            q.Dequeue();
                            var id = dic[(NonTerminal)u[0]];
                            for (int j = 0; j < id.Count; j++)
                            {
                                int tx = id[j];
                                if (ReferenceEquals(items[tx].Right[0][0], dotTerminal))
                                {
                                    if (has[tx])
                                    {
                                        continue;
                                    }
                                    has[tx] = true;
                                    if ((items[tx].Right[0][1] is NonTerminal nt))
                                    {
                                        q.Enqueue(items[tx].Right[0].SubExpr(1, 1));
                                    }
                                    element.Add(items[tx].Right[0]);
                                }
                            }
                        }
                    }
                    Collections.Add(temp);
                }
            }

            for (int i = 0; i < Collections.Count; i++)
            {
                var temp = new Dictionary<AbstractTerminal, Closure>();
                for (int j = 0; j < Collections[i].Count; j++)
                {
                    var str = Collections[i][j].Clone();
                    int x = 0;
                    for (; x < str.Count; x++)
                    {
                        if (ReferenceEquals(str[x], dotTerminal))
                        {
                            break;
                        }
                    }

                    if (x == str.Count - 1)
                    {
                        continue;
                    }
                    var y = str[x + 1];
                    int ii = 0;
                    str.RemoveAt(x);
                    str.Insert(x + 1, dotTerminal);
                    var cmp = new Production(Collections[i][j].Parent.Left);
                    cmp.Add(str);
                    for (int k = 0; k < items.Count; k++)
                    {
                        if (items[k] == cmp)
                        {
                            ii = k;
                            break;
                        }
                    }

                    has = new bool[500];
                    if (!temp.ContainsKey(y))
                    {
                        temp.Add(y, new Closure());
                    }
                    var element = temp[y].Element;
                    element.Add(items[ii].Right[0]);
                    has[ii] = true;
                    x++;
                    if (x != str.Count - 1)
                    {
                        var q = new Queue<ProductionExpression>();
                        q.Enqueue(str.SubExpr(x + 1, 1));
                        while (!(q.Count == 0))
                        {
                            var u = q.Peek();
                            q.Dequeue();
                            List<int> id;
                            if (u[0] is NonTerminal nts)
                            {
                                id = dic[nts];
                                for (int d = 0; d < id.Count; d++)
                                {
                                    int tx = id[d];
                                    if (ReferenceEquals(items[tx].Right[0][0], dotTerminal))
                                    {
                                        if (has[tx])
                                        {
                                            continue;
                                        }
                                        has[tx] = true;
                                        if (items[tx].Right[0].Count > 1 && items[tx].Right[0][1] is NonTerminal nt)
                                        {
                                            q.Enqueue(items[tx].Right[0].SubExpr(1, 1));
                                        }

                                        element.Add(items[tx].Right[0]);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var kp in temp)
                {
                    Collections.Add(kp.Value);
                }
                for (int k = 0; k < Collections.Count; k++)
                {
                    for (int j = k + 1; j < Collections.Count; j++)
                    {
                        if (Collections[k] == Collections[j])
                        {
                            Collections.RemoveAt(j);
                        }
                    }
                }
            }
        }

        private void makeV()
        {
            var used = new Dictionary<AbstractTerminal, bool>();
            for (int i = 0; i < Grammar.Productions.Count; i++)
            {
                var r = Grammar.Productions[i].Right;
                var l = Grammar.Productions[i].Left;
                if (!used.ContainsKey(l))
                {
                    used.Add(l, true);
                    V.Add(l);
                }
                foreach (var str in r)
                {
                    for (int j = 0; j < str.Count; j++)
                    {
                        if (used.ContainsKey(str[j]) && used[str[j]])
                        {
                            continue;
                        }
                        used.Add(str[j], true);
                        V.Add(str[j]);
                    }
                }
            }
            V.Add(Terminal.LexEnd);
        }

        public void MakeGo()
        {
            makeGo();
            var m = Collections.Count;
            foreach (var i in Go)
            {
                for (int j = 0; j < m; j++)
                {
                    foreach (var k in i.Value)
                    {
                        if (k.Value == j)
                        {
                            Console.WriteLine("I{0}--{1}--I{2}", i.Key, k.Key, j);
                        }
                    }
                }
            }

        }

        private void makeCmp(List<Production> cmp1, int i, AbstractTerminal ch)
        {
            for (int j = 0; j < Collections[i].Count; j++)
            {
                var str = Collections[i][j].Clone();
                int k;
                for (k = 0; k < str.Count; k++)
                {
                    if (ReferenceEquals(str[k], dotTerminal))
                    {
                        break;
                    }
                }
                if (k != str.Count - 1 && str[k + 1] == ch)
                {
                    str.RemoveAt(k);
                    str.Insert(k + 1, dotTerminal);
                    var pd = new Production(Collections[i][j].Parent.Left);
                    pd.Add(str);
                    cmp1.Add(pd);
                }
            }
        }

        private void makeGo()
        {
            int m = Collections.Count;
            for (int t = 0; t < V.Count; t++)
            {
                var ch = V[t];
                for (int i = 0; i < m; i++)
                {
                    var cmp1 = new List<Production>();
                    makeCmp(cmp1, i, ch);
                    if (cmp1.Count == 0)
                    {
                        continue;
                    }
                    for (int j = 0; j < m; j++)
                    {
                        var cmp2 = new List<Production>();
                        for (int k = 0; k < Collections[j].Count; k++)
                        {
                            var str = Collections[j][k];
                            int x;
                            for (x = 0; x < str.Count; x++)
                            {
                                if (ReferenceEquals(str[x], dotTerminal))
                                {
                                    break;
                                }
                            }
                            if (x > 0 && str[x - 1] == ch)
                            {
                                var pd = new Production(Collections[j][k].Parent.Left);
                                pd.Add(str);
                                cmp2.Add(pd);
                            }
                        }
                        bool flag = true;
                        if (cmp2.Count != cmp1.Count)
                        {
                            continue;
                        }
                        for (int k = 0; k < cmp1.Count; k++)
                        {
                            if (cmp1[k] == cmp2[k])
                            {
                                continue;
                            }
                            else
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            if (!Go.ContainsKey(i))
                            {
                                Go.Add(i, new Dictionary<AbstractTerminal, int>());
                            }
                            if (!Go[i].ContainsKey(ch))
                            {
                                Go[i].Add(ch, j);
                            }
                        }
                    }
                }
            }
        }

        protected virtual void makeTable()
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

        public void MakeTable()
        {
            makeTable();
            Console.WriteLine("Action:");
            foreach (var k1 in Action)
            {
                Console.WriteLine("\tState: {0}", k1.Key);
                foreach (var k2 in k1.Value)
                {
                    var cont = k2.Value;
                    Console.Write("\t\t{0}: ", k2.Key.Name);
                    switch (cont.ActionType)
                    {
                        case LRTableType.Shift:
                            Console.Write("S");
                            Console.WriteLine(cont.ShiftNextState);
                            break;
                        case LRTableType.Reduce:
                            Console.Write("R");
                            Console.WriteLine("<{0}, {1}>", cont.ReduceProductionExpression.Item1, cont.ReduceProductionExpression.Item2);
                            break;
                        case LRTableType.Accept:
                            Console.WriteLine("Accept");
                            break;
                    }
                }
            }

            Console.WriteLine("Goto:");
            foreach (var k1 in Goto)
            {
                Console.WriteLine("\tState: {0}", k1.Key);
                foreach (var k2 in k1.Value)
                {
                    Console.WriteLine("\t\t{0}:{1}", k2.Key.Name, k2.Value);
                }
            }
        }

        private void printStep(int step, int pointer, List<int> state, Stack<AbstractTerminal> op, List<Token> input, Content action)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}\t", step);
            foreach (var i in state)
            {
                sb.AppendFormat("[{0}]", i);
            }
            sb.Append("\t");
            var arr = op.ToList();
            arr.Reverse();
            foreach (var i in arr)
            {
                sb.AppendFormat("[{0}]", i.Name);
            }
            sb.Append("\t");
            var arrInput = input.Skip(pointer);
            foreach (var item in arrInput)
            {
                sb.Append("[");
                sb.Append(item.Text);
                sb.Append("]");
            }
            sb.Append("\t");
            sb.Append(action.ActionType.ToString());
            if (action.ActionType == LRTableType.Reduce)
            {
                sb.AppendFormat(" ({0})", Grammar.Productions[action.ReduceProductionExpression.Item1].Right[action.ReduceProductionExpression.Item2].ToString(true));
            }

            Console.WriteLine(sb.ToString());
        }

        public override bool Parse(List<Token> input, int textLength = -1)
        {
            var opStack = new Stack<AbstractTerminal>();
            var stateStack = new List<int>();
            input.Add(new Token() { Type = Tokens.LexEnd, Text = "#", EndLocation = textLength });
            opStack.Push(Terminal.LexEnd);
            stateStack.Add(0);
            int steps = 1;
            for (int i = 0; i < input.Count; i++)
            {
                var token = input[i];
                var top = stateStack[stateStack.Count - 1];
                var term = TypeConventer.ToTerminal(token);
                if (!Action[top].ContainsKey(term))
                {
                    ErrorHandler(steps, token);
                    return false;
                }
                var act = Action[top][term];
                switch (act.ActionType)
                {
                    case LRTableType.Shift:
                        printStep(steps++, i, stateStack, opStack, input, act);
                        opStack.Push(term);
                        stateStack.Add(act.ShiftNextState);
                        break;
                    case LRTableType.Reduce:
                        var tt = Grammar.Productions[act.ReduceProductionExpression.Item1].Right[act.ReduceProductionExpression.Item2];
                        int y = stateStack[stateStack.Count - tt.Count - 1];
                        if (!Goto[y].ContainsKey(tt.Parent.Left))
                        {
                            ErrorHandler(steps, token);
                            return false;
                        }
                        int x = Goto[y][tt.Parent.Left];
                        printStep(steps++, i, stateStack, opStack, input, act);
                        for (int j = 0; j < tt.Count; j++)
                        {
                            stateStack.RemoveAt(stateStack.Count - 1);
                            opStack.Pop();
                        }
                        stateStack.Add(x);
                        opStack.Push(tt.Parent.Left);
                        i--;
                        break;
                    case LRTableType.Accept:
                        printStep(steps++, i, stateStack, opStack, input, act);
                        return true;
                }
            }
            return false;
        }
    }
}
