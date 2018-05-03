namespace Compiler.Syntactic.LR
{
    internal enum LRTableType
    {
        Shift = 0,
        Reduce = 1,
        Accept = 2,
        Error = 3
    }
}
