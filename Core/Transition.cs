namespace StateMachine.Core
{
    internal readonly ref struct Transition<TFrom, TTo>(TFrom from, TTo to)
        where TFrom : notnull
        where TTo : notnull
    {
        public readonly TFrom from = from;
        public readonly TTo to = to;
        public Transition<TTo, TFrom> Reversed => new(to, from);
    }
}
