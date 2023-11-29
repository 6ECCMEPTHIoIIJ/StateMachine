using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateMachine<TState, TTrigger> _machine;
        private readonly StateTransitionConfigurer<TState, TTrigger> _transitionConfigurer;

        internal TState? State { private get; set; }

        internal StateConfigurer(StateMachine<TState, TTrigger> machine)
        {
            _machine = machine;
            _transitionConfigurer = new StateTransitionConfigurer<TState, TTrigger>(_machine);
        }

        [Pure]
        public StateTransitionConfigurer<TState, TTrigger> Permit(TState to, TTrigger trigger)
        {
            _transitionConfigurer.From = State;
            _transitionConfigurer.To = to;
            _transitionConfigurer.Trigger = trigger;
            return _transitionConfigurer;
        }

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

        public StateConfigurer<TState, TTrigger> OnProcess(Action action)
        {
            _machine.OnProcessTable.AddActionOnProcess(State, action);
            return this;
        }
    }
}
