using StateMachine.Core;

using System;
using System.Diagnostics.Contracts;

namespace StateMachine
{
    public abstract class StateActionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly StateMachine<TState, TTrigger> _machine;
        private readonly ActionTable<TState> _actionTable;
        private readonly StateReactionConfigurer<TState, TTrigger> _reactionConfigurer;

        internal TState State { private get; set; }
        internal Action? Action { private get; set; }

        internal StateActionConfigurer(StateMachine<TState, TTrigger> machine, ActionTable<TState> actionTable)
        {
            _machine = machine;
            _actionTable = actionTable;
            _reactionConfigurer = new(machine, actionTable);
        }

        public StateConfigurer<TState, TTrigger> Default()
        {
            _actionTable.AddAction(State, Action!);
            return _machine.ContinueConfiguration();
        }

        [Pure]
        protected StateReactionConfigurer<TState, TTrigger> On(TState on)
        {
            _reactionConfigurer.State = State;
            _reactionConfigurer.Action = Action;
            _reactionConfigurer.On = on;
            return _reactionConfigurer;
        }
    }
}