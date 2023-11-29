using StateMachine.Core;

using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        internal StateMachine<TState, TTrigger> Machine { get; }
        internal TState State { get; }

        internal StateConfigurer(StateMachine<TState, TTrigger> machine, TState state)
        {
            Machine = machine;
            State = state;
        }

        [Pure]
        public StateTransitionConfigurer<TState, TTrigger> Permit(TState to, TTrigger trigger) => new(this, to, trigger);

        [Pure]
        public StateOnEntryConfigurer<TState, TTrigger> OnEntry(Action action) => new(this, action);

        [Pure]
        public StateOnExitConfigurer<TState, TTrigger> OnExit(Action action) => new(this, action);

        [Pure]
        public StateIgnoreTransitionConfigurer<TState, TTrigger> Ignore(TTrigger trigger) => new(this, trigger);

        [Pure]
        public StateIgnoreOnEntryConfigurer<TState, TTrigger> IgnoreEntry() => new(this);
    }

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
