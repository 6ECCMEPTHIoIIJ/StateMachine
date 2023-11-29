using StateMachine.Core;

namespace StateMachine
{
    public class StateReactionOnEntryConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateOnEntryConfigurer<TState, TTrigger> _configurer;
        private readonly TState _from;

        internal StateReactionOnEntryConfigurer(StateOnEntryConfigurer<TState, TTrigger> configurer, TState from)
        {
            _configurer = configurer;
            _from = from;
        }

        public StateConfigurer<TState, TTrigger> OverrideDefault()
        {
            AddReactionOnEntryFrom(StateReactionBehaviour.OverrideDefault);
            return _configurer.Configurer;
        }

        public StateConfigurer<TState, TTrigger> BeforeDefault()
        {
            AddReactionOnEntryFrom(StateReactionBehaviour.BeforeDefault);
            return _configurer.Configurer;
        }

        public StateConfigurer<TState, TTrigger> AfterDefault()
        {
            AddReactionOnEntryFrom(StateReactionBehaviour.AfterDefault);
            return _configurer.Configurer;
        }

        private void AddReactionOnEntryFrom(StateReactionBehaviour reactionBehaviour)
            => _configurer.Configurer.Machine.OnEntryTable.AddReactionOnEntryFrom(
                new Transition<TState, TState>(_from, _configurer.Configurer.State),
                new StateReaction(_configurer.Action, reactionBehaviour));
    }
}
