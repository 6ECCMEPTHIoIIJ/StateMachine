using StateMachine.Core;

namespace StateMachine
{
    public class StateTransitionConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateConfigurer<TState, TTrigger> _configurer;
        private readonly TState _to;
        private readonly TTrigger _trigger;

        internal StateTransitionConfigurer(StateConfigurer<TState, TTrigger> configurer, TState to, TTrigger trigger)
        {
            _configurer = configurer;
            _to = to;
            _trigger = trigger;
        }

        public StateConfigurer<TState, TTrigger> Unconditional()
        {
            var transition = new Transition<TState, TTrigger>(_configurer.State, _trigger);
            _configurer.Machine.TransitionTable.AddTransition(transition, _to);
            return _configurer;
        }

        public StateConfigurer<TState, TTrigger> If(Func<bool> condition)
        {
            var transition = new Transition<TState, TTrigger>(_configurer.State, _trigger);
            _configurer.Machine.TransitionTable.AddTransition(transition, _to);
            _configurer.Machine.TransitionTable.AddCondition(transition, condition);
            return _configurer;
        }
    }
}
