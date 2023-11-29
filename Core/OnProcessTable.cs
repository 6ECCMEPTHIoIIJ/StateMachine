using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class OnProcessTable<TState>
        where TState : struct
    {
        private readonly Dictionary<TState, Action> _onProcesses = [];

        public bool TryGetActionOnProcess(TState state, [MaybeNullWhen(false)] out Action action)
            => _onProcesses.TryGetValue(state, out action);

        public void AddActionOnProcess(TState state, Action action)
            => _onProcesses[state] = action;

        public bool RemoveActionOnProcess(TState state)
            => _onProcesses.Remove(state);
    }
}
