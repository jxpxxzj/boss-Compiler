using System;

namespace Compiler.Syntactic.LR
{

    internal class Content
    {
        public LRTableType ActionType;
        public int ShiftNextState;
        public Tuple<int, int> ReduceProductionExpression;

        public Content(LRTableType a, int b)
        {
            ActionType = a;
            ShiftNextState = b;
            ReduceProductionExpression = null;
        }
    }
}
