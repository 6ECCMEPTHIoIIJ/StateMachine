using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class TransitionTable<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly Relation<TState, TTrigger, TState> _transitions = [];
        private readonly Relation<TState, TTrigger, Func<bool>> _conditions = [];

        public bool TryGetTransition(Transition<TState, TTrigger> transition, [MaybeNullWhen(false)] out TState to)
            => _transitions.TryGetValue(transition, out to)
            && (!_conditions.TryGetValue(transition, out var condition)
            || condition.Invoke());

        public void AddTransition(Transition<TState, TTrigger> transition, TState to)
            => _transitions[transition] = to;

        public void AddCondition(Transition<TState, TTrigger> transition, Func<bool> when)
            => _conditions[transition] = when;

        public void RemoveTransition(Transition<TState, TTrigger> transition)
            => _transitions.Remove(transition);

        public void RemoveCondition(Transition<TState, TTrigger> transition)
            => _conditions.Remove(transition);
    }
}
