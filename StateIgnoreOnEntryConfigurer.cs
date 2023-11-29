using StateMachine.Core;

namespace StateMachine
{
    public class StateIgnoreOnEntryConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateConfigurer<TState, TTrigger> _configurer;

        internal StateIgnoreOnEntryConfigurer(StateConfigurer<TState, TTrigger> configurer)
        {
            _configurer = configurer;
        }

        public StateConfigurer<TState, TTrigger> Default()
        {
            _configurer.Machine.OnEntryTable.RemoveActionOnEntry(_configurer.State);
            return _configurer;
        }

        public StateConfigurer<TState, TTrigger> From(TState from)
        {
            _configurer.Machine.OnEntryTable.RemoveReactionOnEntryFrom(new Transition<TState, TState>(from, _configurer.State));
            return _configurer;
        }

        public StateConfigurer<TState, TTrigger> FromAll()
        {
            _configurer.Machine.OnEntryTable.RemoveReactionsOnEntry(_configurer.State);
            return _configurer;
        }
    }
}
