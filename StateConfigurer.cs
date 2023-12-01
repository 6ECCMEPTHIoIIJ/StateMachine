using System;
using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly StateMachine<TState, TTrigger> _machine;
        private readonly StateTransitionConfigurer<TState, TTrigger> _transitionConfigurer;
        private readonly StateOnEntryConfigurer<TState, TTrigger> _onEntryConfigurer;
        private readonly StateOnExitConfigurer<TState, TTrigger> _OnExitConfigurer;

        internal TState State { private get; set; }

        internal StateConfigurer(StateMachine<TState, TTrigger> machine)
        {
            _machine = machine;
            _transitionConfigurer = new(machine);
            _onEntryConfigurer = new(machine);
            _OnExitConfigurer = new(machine);
        }

        [Pure]
        public StateTransitionConfigurer<TState, TTrigger> Permit(TState to, TTrigger trigger)
        {
            _transitionConfigurer.From = State;
            _transitionConfigurer.To = to;
            _transitionConfigurer.Trigger = trigger;
            return _transitionConfigurer;
        }

        [Pure]
        public StateOnEntryConfigurer<TState, TTrigger> OnEntry(Action action)
        {
            _onEntryConfigurer.State = State;
            _onEntryConfigurer.Action = action;
            return _onEntryConfigurer;
        }

        [Pure]
        public StateOnExitConfigurer<TState, TTrigger> OnExit(Action action)
        {
            _OnExitConfigurer.State = State;
            _OnExitConfigurer.Action = action;
            return _OnExitConfigurer;
        }

        public StateConfigurer<TState, TTrigger> OnProcess(Action action)
        {
            _machine.OnProcessTable.AddAction(State, action);
            return this;
        }
    }
}
