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
        private readonly StateIgnoreTransitionConfigurer<TState, TTrigger> _ignoreTransitionConfigurer;
        private readonly StateIgnoreOnEntryConfigurer<TState, TTrigger> _ignoreOnEntryConfigurer;
        private readonly StateIgnoreOnExitConfigurer<TState, TTrigger> _ignoreOnExitConfigurer;

        internal TState State { private get; set; }

        internal StateConfigurer(StateMachine<TState, TTrigger> machine)
        {
            _machine = machine;
            _transitionConfigurer = new(machine);
            _onEntryConfigurer = new(machine);
            _OnExitConfigurer = new(machine);
            _ignoreTransitionConfigurer = new(machine);
            _ignoreOnEntryConfigurer = new(machine);
            _ignoreOnExitConfigurer = new(machine);
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

        [Pure]
        public StateIgnoreTransitionConfigurer<TState, TTrigger> Ignore(TTrigger trigger)
        {
            _ignoreTransitionConfigurer.From = State;
            _ignoreTransitionConfigurer.Trigger = trigger;
            return _ignoreTransitionConfigurer;
        }

        [Pure]
        public StateIgnoreOnEntryConfigurer<TState, TTrigger> IgnoreEntry()
        {
            _ignoreOnEntryConfigurer.State = State;
            return _ignoreOnEntryConfigurer;
        }

        [Pure]
        public StateIgnoreOnExitConfigurer<TState, TTrigger> IgnoreExit()
        {
            _ignoreOnExitConfigurer.State = State;
            return _ignoreOnExitConfigurer;
        }

        public StateConfigurer<TState, TTrigger> OnProcess(Action action)
        {
            _machine.OnProcessTable.AddActionOnProcess(State, action);
            return this;
        }

        public StateConfigurer<TState, TTrigger> IgnoreProcess()
        {
            _machine.OnProcessTable.RemoveActionOnProcess(State!);
            return this;
        }

        public StateConfigurer<TState, TTrigger> AsSubstateOf(TState supersate)
        {
            _machine.SubstateTable.AddSubstateOf(State, supersate);
            return this;
        }

        public StateConfigurer<TState, TTrigger> AsSuperstateOf(TState substate)
        {
            _machine.SubstateTable.AddSubstateOf(substate, State);
            return this;
        }

        public StateConfigurer<TState, TTrigger> IgnoreSubstates()
        {
            _machine.SubstateTable.RemoveSuperstate(State);
            return this;
        }

        public StateConfigurer<TState, TTrigger> IgnoreSuperstate()
        {
            _machine.SubstateTable.RemoveSubstate(State);
            return this;
        }
    }
}
