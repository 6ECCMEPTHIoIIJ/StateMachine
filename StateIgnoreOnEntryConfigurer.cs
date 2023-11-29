using StateMachine.Core;

namespace StateMachine
{
    public class StateIgnoreOnEntryConfigurer<TState, TTrigger>
                : StateIgnoreActionConfigurer<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        internal StateIgnoreOnEntryConfigurer(StateMachine<TState, TTrigger> machine) : base(machine, machine.OnEntryTable)
        {
        }

        public StateConfigurer<TState, TTrigger> From(TState from) => On(from);

        public StateConfigurer<TState, TTrigger> FromAll() => OnAll();
    }
}
