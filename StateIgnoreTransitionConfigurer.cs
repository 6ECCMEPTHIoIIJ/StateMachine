namespace StateMachine
{
    public class StateIgnoreTransitionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly StateMachine<TState, TTrigger> _machine;

        internal TState From { private get; set; }
        internal TTrigger Trigger { private get; set; }

        internal StateIgnoreTransitionConfigurer(StateMachine<TState, TTrigger> machine)
        {
            _machine = machine;
        }

        public StateConfigurer<TState, TTrigger> Unconditional()
        {
            _machine.TransitionTable.RemoveTransition(From, Trigger);
            _machine.TransitionTable.RemoveCondition(From, Trigger);
            return _machine.ContinueConfiguration();
        }

        public StateConfigurer<TState, TTrigger> If()
        {
            _machine.TransitionTable.RemoveCondition(From, Trigger);
            return _machine.ContinueConfiguration();
        }
    }
}
