using StateMachine.Core;

using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateMachine<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private TState _current;

        internal TransitionTable<TState, TTrigger> TransitionTable { get; } = new();
        internal OnEntryTable<TState> OnEntryTable { get; } = new();
        internal OnExitTable<TState> OnExitTable { get; } = new();
        internal OnProcessTable<TState> OnProcessTable { get; } = new();
        internal SubstateTable<TState> SubstateTable { get; } = new();
        internal StateConfigurer<TState, TTrigger> Configurer { get; }

        public StateMachine(TState initialState)
        {
            _current = initialState;
            Configurer = new StateConfigurer<TState, TTrigger>(this);
        }

        [Pure]
        public StateConfigurer<TState, TTrigger> Configure(TState state)
        {
            Configurer.State = state;
            return Configurer;
        }

        [Pure]
        public bool IsInState(TState state) => _current.Equals(state);

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
                if (OnExitTable.TryGetActionOnExit(transition, out var action))
                    action.Invoke();
                if (SubstateTable.TryGetSuperstate(transition.from, out var superstate))
                    ExitFromState(new Transition<TState, TState>(superstate, transition.to));
            }

            void EnterToState(Transition<TState, TState> transition)
            {
                if (SubstateTable.TryGetSuperstate(transition.to, out var superstate))
                    EnterToState(new Transition<TState, TState>(transition.from, superstate));
                if (OnEntryTable.TryGetActionOnEntry(transition, out var action))
                    action.Invoke();
            }
        }
    }
}
