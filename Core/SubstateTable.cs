using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class SubstateTable<TState>
        where TState : notnull
    {
        private readonly Dictionary<TState, TState> _substates = [];

        public bool TryGetSuperstate(TState substate, [MaybeNullWhen(false)] out TState superstate)
            => _substates.TryGetValue(substate, out superstate);

        public void AddSubstateOf(Transition<TState, TState> transition)
            => _substates[transition.from] = transition.to;

        public void AddSuperstateOf(Transition<TState, TState> transition)
            => _substates[transition.to] = transition.from;

        public void RemoveSubstate(TState substate)
            => _substates.Remove(substate);

        public void RemoveSuperstate(TState superstate)
        {
            foreach (var state in _substates)
                if (state.Value.Equals(superstate))
                    _substates.Remove(state.Key);

        }
    }
}
