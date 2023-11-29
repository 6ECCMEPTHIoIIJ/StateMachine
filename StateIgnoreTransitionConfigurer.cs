using StateMachine.Core;

namespace StateMachine
{
    public class StateIgnoreTransitionConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateConfigurer<TState, TTrigger> _configurer;
        private readonly TTrigger _trigger;

        internal StateIgnoreTransitionConfigurer(StateConfigurer<TState, TTrigger> configurer, TTrigger trigger)
        {
            _configurer = configurer;
            _trigger = trigger;
        }

        public StateConfigurer<TState, TTrigger> Unconditional()
        {
            var transition = new Transition<TState, TTrigger>(_configurer.State, _trigger);
            _configurer.Machine.TransitionTable.RemoveTransition(transition);
            _configurer.Machine.TransitionTable.RemoveCondition(transition);
            return _configurer;
        }

        public StateConfigurer<TState, TTrigger> If()
        {
            var transition = new Transition<TState, TTrigger>(_configurer.State, _trigger);
            _configurer.Machine.TransitionTable.RemoveCondition(transition);
            return _configurer;
        }
    }
}
