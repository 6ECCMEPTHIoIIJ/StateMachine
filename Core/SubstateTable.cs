using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class SubstateTable<TState>
        where TState : struct
    {
        private readonly Dictionary<TState, TState> _substates = [];

        public bool TryGetSuperstate(TState substate, [MaybeNullWhen(false)] out TState superstate)
            => _substates.TryGetValue(substate, out superstate);

        public void AddSubstateOf(TState substate, TState superstate)
            => _substates[substate] = superstate;

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
