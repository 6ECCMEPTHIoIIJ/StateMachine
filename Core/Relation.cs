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
            set
            {
                if (!TryGetValue(from, out var transitions))
                    this[from] = transitions = [];
                transitions[to] = value;
            }
        }

        public bool TryGetValue(TFrom from, TTo to, [MaybeNullWhen(false)] out TValue value)
        {
            value = default;
            return TryGetValue(from, out var transitions)
                && transitions.TryGetValue(to, out value);
        }

        public bool Remove(TFrom from, TTo to)
            => TryGetValue(from, out var transitions)
            && transitions.Remove(to);
    }
}
