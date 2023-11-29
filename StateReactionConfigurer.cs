using StateMachine.Core;

using System;

namespace StateMachine
{
    public class StateReactionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly StateMachine<TState, TTrigger> _machine;
        private readonly ActionTable<TState> _actionTable;

        internal TState State { private get; set; }
        internal TState On { private get; set; }
        internal Action? Action { private get; set; }

        internal StateReactionConfigurer(StateMachine<TState, TTrigger> machine, ActionTable<TState> actionTable)
        {
            _machine = machine;
            _actionTable = actionTable;
        }

        public StateConfigurer<TState, TTrigger> OverrideDefault()
        {
            AddReactionOnEntryFrom(StateReactionBehaviours.OverrideDefault);
            return _machine.ContinueConfiguration();
        }

        public StateConfigurer<TState, TTrigger> BeforeDefault()
        {
            AddReactionOnEntryFrom(StateReactionBehaviours.BeforeDefault);
            return _machine.ContinueConfiguration();
        }

        public StateConfigurer<TState, TTrigger> AfterDefault()
        {
            AddReactionOnEntryFrom(StateReactionBehaviours.AfterDefault);
            return _machine.ContinueConfiguration();
        }

        private void AddReactionOnEntryFrom(StateReactionBehaviours reactionBehaviour)
            => _actionTable.AddReaction(State, On,
                new StateReaction
                {
                    Action = Action!,
                    Behaviour = reactionBehaviour
                });
    }
}
