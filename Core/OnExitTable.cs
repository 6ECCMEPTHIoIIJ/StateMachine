using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class OnExitTable<TState>
        where TState : notnull
    {
        private readonly Relation<TState, TState, StateReaction> _onExitsTo = [];
        private readonly Dictionary<TState, Action> _onExits = [];

        public bool TryGetActionOnExit(Transition<TState, TState> transition, [MaybeNullWhen(false)] out Action action)
        {
            bool hasReactionOnExitTo = _onExitsTo.TryGetValue(transition, out var reactionOnExitTo);
            bool hasActionOnExit = _onExits.TryGetValue(transition.from, out var actionOnExit);
            if (!hasReactionOnExitTo && hasActionOnExit)
                action = actionOnExit;
            else if (hasReactionOnExitTo && !hasActionOnExit)
                action = reactionOnExitTo.action;
            else if (hasReactionOnExitTo && hasActionOnExit)
                action = reactionOnExitTo.behaviour switch
                {
                    StateReactionBehaviours.OverrideDefault => reactionOnExitTo.action,
                    StateReactionBehaviours.AfterDefault => actionOnExit + reactionOnExitTo.action,
                    StateReactionBehaviours.BeforeDefault => reactionOnExitTo.action + actionOnExit,
                    _ => throw new NotImplementedException(),
                };
            else
                action = null;

            return hasReactionOnExitTo || hasActionOnExit;
        }

        public void AddActionOnExit(TState from, Action action)
            => _onExits[from] = action;

        public void AddReactionOnExitTo(Transition<TState, TState> transition, StateReaction reaction)
            => _onExitsTo[transition] = reaction;

        public void RemoveActionOnExit(TState from)
            => _onExits.Remove(from);

        public void RemoveReactionOnExitTo(Transition<TState, TState> transition)
            => _onExitsTo.Remove(transition);

        public void RemoveReactionsOnExit(TState from)
            => _onExitsTo.Remove(from);
    }
}
