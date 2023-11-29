using StateMachine.Core;

using System;
using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateOnEntryConfigurer<TState, TTrigger>
        : StateActionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        internal StateOnEntryConfigurer(StateMachine<TState, TTrigger> machine) 
            : base(machine, machine.OnEntryTable)
        {
        }

        [Pure]
        public StateReactionConfigurer<TState, TTrigger> From(TState from) => On(from);
    }
}
