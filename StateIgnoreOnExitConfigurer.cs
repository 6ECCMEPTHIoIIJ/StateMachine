using StateMachine.Core;

namespace StateMachine
{
    public class StateIgnoreOnExitConfigurer<TState, TTrigger>
        : StateIgnoreActionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        internal StateIgnoreOnExitConfigurer(StateMachine<TState, TTrigger> machine) : base(machine, machine.OnExitTable)
        {
        }

        public StateConfigurer<TState, TTrigger> To(TState to) => On(to);

        public StateConfigurer<TState, TTrigger> ToAll() => OnAll();
    }
}
