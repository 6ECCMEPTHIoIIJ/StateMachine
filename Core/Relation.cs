using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class Relation<TFrom, TTo, TValue>
        : Dictionary<TFrom, Dictionary<TTo, TValue>>
        where TFrom : struct
        where TTo : struct
    {
        public TValue this[TFrom from, TTo to]
        {
            get => this[from][to];
            set => this[from][to] = value;
        }

        public bool TryGetValue(TFrom from, TTo to, [MaybeNullWhen(false)] out TValue value) 
            => this[from].TryGetValue(to, out value);
    }
}
