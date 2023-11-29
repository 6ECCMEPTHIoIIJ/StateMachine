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

        [Pure]
        public StateIgnoreOnExitConfigurer<TState, TTrigger> IgnoreExit() => new(this);
    }
}
