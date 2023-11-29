using System;

namespace StateMachine
{
    public class StateTransitionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly StateMachine<TState, TTrigger> _machine;

        internal TState From { private get; set; }
        internal TState To { private get; set; }
        internal TTrigger Trigger { private get; set; }

        internal StateTransitionConfigurer(StateMachine<TState, TTrigger> machine)
        {
            _machine = machine;
        }

        public StateConfigurer<TState, TTrigger> Unconditional()
        {
            _machine.TransitionTable.AddTransition(From, Trigger, To);
            return _machine.ContinueConfiguration();
        }

        public StateConfigurer<TState, TTrigger> If(Func<bool> condition)
        {
            _machine.TransitionTable.AddTransition(From, Trigger, To);
            _machine.TransitionTable.AddCondition(From, Trigger, condition);
            return _machine.ContinueConfiguration();
        }
    }
}
