using StateMachine.Core;

using System.Diagnostics.Contracts;

namespace StateMachine
{
    public class StateMachine<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        private readonly StateConfigurer<TState, TTrigger> _configurer;
        private TState _current;

        internal TransitionTable<TState, TTrigger> TransitionTable { get; } = new();
        internal OnEntryTable<TState> OnEntryTable { get; } = new();
        internal OnExitTable<TState> OnExitTable { get; } = new();
        internal OnProcessTable<TState> OnProcessTable { get; } = new();

        public StateMachine(TState current)
        {
            _current = current;
            _configurer = new(this);
        }

        [Pure]
        public StateConfigurer<TState, TTrigger> Configure(TState state)
        {
            InitializeStateInternal(state);
            _configurer.State = state;
            return _configurer;
        }

        [Pure]
        public bool IsInState(TState state) => _current.Equals(state);

        public void Fire(TTrigger trigger)
        {
            if (TransitionTable.TryGetTransition(_current, trigger, out var to))
            {
                ExitFromStateInternal(_current, to);
                EnterToStateInternal(_current, to);
                _current = to;
            }
        }

        public void Process()
        {
            if (OnProcessTable.TryGetAction(_current, out var action))
                action.Invoke();
        }

        [Pure]
        internal StateConfigurer<TState, TTrigger> ContinueConfiguration() 
            => _configurer;

        private void ExitFromStateInternal(TState from, TState to)
        {
            if (OnExitTable.TryGetAction(from, to, out var action))
                action.Invoke();
        }

        private void EnterToStateInternal(TState from, TState to)
        {
            if (OnEntryTable.TryGetAction(to, from, out var action))
                action.Invoke();
        }

        private void InitializeStateInternal(TState state)
        {
            TransitionTable.AddState(state);
            OnExitTable.AddState(state);
            OnEntryTable.AddState(state);
            OnProcessTable.AddState(state);
        }
    }
}
