using StateMachine.Core;

namespace StateMachine
{
    public class StateIgnoreOnExitConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateConfigurer<TState, TTrigger> _configurer;

        internal StateIgnoreOnExitConfigurer(StateConfigurer<TState, TTrigger> configurer)
        {
            _configurer = configurer;
        }

        public StateConfigurer<TState, TTrigger> Default()
        {
            _configurer.Machine.OnExitTable.RemoveActionOnExit(_configurer.State);
            return _configurer;
        }

        public StateConfigurer<TState, TTrigger> To(TState to)
        {
            _configurer.Machine.OnExitTable.RemoveReactionOnExitTo(new Transition<TState, TState>(_configurer.State, to));
            return _configurer;
        }

        public StateConfigurer<TState, TTrigger> ToAll()
        {
            _configurer.Machine.OnExitTable.RemoveReactionsOnExit(_configurer.State);
            return _configurer;
        }
    }
}
