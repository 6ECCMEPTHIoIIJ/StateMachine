using StateMachine.Core;

namespace StateMachine
{
    public class StateTransitionConfigurer<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly StateMachine<TState, TTrigger> _machine;

        internal TState? From { private get; set; }
        internal TState? To { private get; set; }
        internal TTrigger? Trigger { private get; set; }

        internal StateTransitionConfigurer(StateMachine<TState, TTrigger> machine)
        {
            _machine = machine;
        }

        public StateConfigurer<TState, TTrigger> Unconditional()
        {
            var transition = new Transition<TState, TTrigger>(From!, Trigger!);
            _machine.TransitionTable.AddTransition(transition, To!);
            return _machine.Configurer;
        }

        public StateConfigurer<TState, TTrigger> If(Func<bool> condition)
        {
            var transition = new Transition<TState, TTrigger>(From!, Trigger!);
            _machine.TransitionTable.AddTransition(transition, To!);
            _machine.TransitionTable.AddCondition(transition, condition);
            return _machine.Configurer;
        }
    }
}
