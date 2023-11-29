using StateMachine.Core;

namespace StateMachine
{
    public class StateReactionOnExitConfigurer<TState, TTrigger>
    where TState : notnull
    where TTrigger : notnull
    {
        private readonly StateOnExitConfigurer<TState, TTrigger> _configurer;
        private readonly TState _to;

        internal StateReactionOnExitConfigurer(StateOnExitConfigurer<TState, TTrigger> configurer, TState to)
        {
            _configurer = configurer;
            _to = to;
        }

        public StateConfigurer<TState, TTrigger> OverrideDefault()
        {
            AddReactionOnExitTo(StateReactionBehaviours.OverrideDefault);
            return _configurer.Configurer;
        }

        public StateConfigurer<TState, TTrigger> BeforeDefault()
        {
            AddReactionOnExitTo(StateReactionBehaviours.BeforeDefault);
            return _configurer.Configurer;
        }

        public StateConfigurer<TState, TTrigger> AfterDefault()
        {
            AddReactionOnExitTo(StateReactionBehaviours.AfterDefault);
            return _configurer.Configurer;
        }

        private void AddReactionOnExitTo(StateReactionBehaviours reactionBehaviour)
            => _configurer.Configurer.Machine.OnExitTable.AddReactionOnExitTo(
                new Transition<TState, TState>(_configurer.Configurer.State, _to),
                new StateReaction(_configurer.Action, reactionBehaviour));
    }
}
