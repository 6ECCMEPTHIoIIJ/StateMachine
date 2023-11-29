using StateMachine.Core;

using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateMachine<TState, TTrigger>(TState initialState)
        where TState : notnull
        where TTrigger : notnull
    {
        private TState _current = initialState;

        internal TransitionTable<TState, TTrigger> TransitionTable { get; } = new();
        internal OnEntryTable<TState> OnEntryTable { get; } = new();
        internal OnExitTable<TState> OnExitTable { get; } = new();
        internal SubstateTable<TState> SubstateTable { get; } = new();

        [Pure]
        public StateConfigurer<TState, TTrigger> Configure(TState state) => new(this, state);

        public void Fire(TTrigger trigger)
        {
            if (TransitionTable.TryGetTransition(new Transition<TState, TTrigger>(_current, trigger), out var to))
                SwitchState(new Transition<TState, TState>(_current, to));

            void SwitchState(Transition<TState, TState> transition)
            {
                ExitFromState(transition);
                EnterToState(transition);
                _current = transition.to;
            }

            void ExitFromState(Transition<TState, TState> transition)
            {
                if (OnExitTable.TryGetOnExitAction(transition, out var action))
                    action.Invoke();
                if (SubstateTable.TryGetSuperstate(transition.from, out var superstate))
                    ExitFromState(new Transition<TState, TState>(superstate, transition.to));
            }

            void EnterToState(Transition<TState, TState> transition)
            {
                if (SubstateTable.TryGetSuperstate(transition.to, out var superstate))
                    EnterToState(new Transition<TState, TState>(transition.from, superstate));
                if (OnEntryTable.TryGetOnEntryAction(transition, out var action))
                    action.Invoke();
            }
        }
    }
}
