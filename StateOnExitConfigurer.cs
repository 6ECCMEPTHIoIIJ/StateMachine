using StateMachine.Core;

using System;
using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateOnExitConfigurer<TState, TTrigger>
        : StateActionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        internal StateOnExitConfigurer(StateMachine<TState, TTrigger> machine) : base(machine, machine.OnExitTable)
        {
        }

        [Pure]
        public StateReactionConfigurer<TState, TTrigger> To(TState to) => On(to);
    }
}
