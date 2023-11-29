using System;
using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class TransitionTable<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly Relation<TState, TTrigger, TState> _transitions = [];
        private readonly Relation<TState, TTrigger, Func<bool>> _conditions = [];

        public bool TryGetTransition(TState from, TTrigger trigger, [MaybeNullWhen(false)] out TState to)
            => _transitions.TryGetValue(from, trigger, out to)
            && (!_conditions.TryGetValue(from, trigger, out var condition)
            || condition.Invoke());

        public void AddTransition(TState from, TTrigger trigger, TState to)
            => _transitions[from, trigger] = to;

        public void AddCondition(TState from, TTrigger trigger, Func<bool> when)
            => _conditions[from, trigger] = when;

        public void RemoveTransition(TState from, TTrigger trigger)
            => _transitions.Remove(from, trigger);

        public void RemoveCondition(TState from, TTrigger trigger)
            => _conditions.Remove(from, trigger);
    }
}
