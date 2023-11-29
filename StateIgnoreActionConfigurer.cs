using StateMachine.Core;

using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateIgnoreActionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly ActionTable<TState> _actionTable;
        private readonly StateMachine<TState, TTrigger> _machine;

        internal TState State { private get; set; }

        internal StateIgnoreActionConfigurer(StateMachine<TState, TTrigger> machine, ActionTable<TState> actionTable)
        {
            _machine = machine;
            _actionTable = actionTable;
        }

        public StateConfigurer<TState, TTrigger> Default()
        {
            _actionTable.RemoveAction(State!);
            return _machine.ContinueConfiguration();
        }

        [Pure]
        protected StateConfigurer<TState, TTrigger> On(TState on)
        {
            _actionTable.RemoveReaction(State!, on);
            return _machine.ContinueConfiguration();
        }

        [Pure]
        protected StateConfigurer<TState, TTrigger> OnAll()
        {
            _actionTable.RemoveReactions(State!);
            return _machine.ContinueConfiguration();
        }
    }
}