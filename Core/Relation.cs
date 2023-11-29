using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class Relation<TFrom, TTo, TValue>
        : Dictionary<TFrom, Dictionary<TTo, TValue>>
        where TFrom : notnull
        where TTo : notnull
    {
        public TValue this[Transition<TFrom, TTo> transition]
        {
            get => this[transition.from][transition.to];
            set
            {
                if (!TryGetValue(transition.from, out var transitions))
                    this[transition.from] = transitions = [];
                transitions[transition.to] = value;
            }
        }

        public bool TryGetValue(Transition<TFrom, TTo> transition, [MaybeNullWhen(false)] out TValue value)
        {
            value = default;
            return TryGetValue(transition.from, out var transitions) 
                && transitions.TryGetValue(transition.to, out value);
        }

        public bool Remove(Transition<TFrom, TTo> transition)
            => TryGetValue(transition.from, out var transitions) 
            && transitions.Remove(transition.to);

        public void Add(Transition<TFrom, TTo> transition, TValue value)
        {
            if (!TryGetValue(transition.from, out var transitions))
                this[transition.from] = transitions = [];
            transitions.Add(transition.to, value);
        }

        public bool TryAdd(Transition<TFrom, TTo> transition, TValue value)
        {
            if (!TryGetValue(transition.from, out var transitions))
                this[transition.from] = transitions = [];
            return transitions.TryAdd(transition.to, value);
        }

        public bool ContainsKey(Transition<TFrom, TTo> transition)
            => TryGetValue(transition.from, out var transitions)
            && transitions.ContainsKey(transition.to);
    }
}
