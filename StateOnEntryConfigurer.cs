using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateOnEntryConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        internal StateConfigurer<TState, TTrigger> Configurer { get; }
        internal Action Action { get; }

        internal StateOnEntryConfigurer(StateConfigurer<TState, TTrigger> configurer, Action action)
        {
            Configurer = configurer;
            Action = action;
        }

        public StateConfigurer<TState, TTrigger> Default()
        {
            Configurer.Machine.OnEntryTable.AddActionOnEntry(Configurer.State, Action);
            return Configurer;
        }

        [Pure]
        public StateReactionOnEntryConfigurer<TState, TTrigger> From(TState from) => new(this, from);

    }
}
