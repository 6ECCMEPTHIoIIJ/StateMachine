using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateOnExitConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        internal StateConfigurer<TState, TTrigger> Configurer { get; }
        internal Action Action { get; }

        internal StateOnExitConfigurer(StateConfigurer<TState, TTrigger> configurer, Action action)
        {
            Configurer = configurer;
            Action = action;
        }

        public StateConfigurer<TState, TTrigger> Default()
        {
            Configurer.Machine.OnExitTable.AddActionOnExit(Configurer.State, Action);
            return Configurer;
        }

        [Pure]
        public StateReactionOnExitConfigurer<TState, TTrigger> To(TState to) => new(this, to);
    }
}
