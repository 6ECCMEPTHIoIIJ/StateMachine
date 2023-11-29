using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class OnEntryTable<TState>
        where TState : notnull
    {
        private readonly Relation<TState, TState, StateReaction> _onEntriesFrom = [];
        private readonly Dictionary<TState, Action> _onEntries = [];

        public bool TryGetOnEntryAction(Transition<TState, TState> transition, [MaybeNullWhen(false)] out Action action)
        {
            bool hasReactionOnEntryFrom = _onEntriesFrom.TryGetValue(transition.Reversed, out var reactionOnEntryFrom);
            bool hasActionOnEntry = _onEntries.TryGetValue(transition.to, out var actionOnEntry);
            if (!hasReactionOnEntryFrom && hasActionOnEntry)
                action = actionOnEntry;
            else if (hasReactionOnEntryFrom && !hasActionOnEntry)
                action = reactionOnEntryFrom.action;
            else if (hasReactionOnEntryFrom && hasActionOnEntry)
                action = reactionOnEntryFrom.behaviour switch
                {
                    StateReactionBehaviour.OverrideDefault => reactionOnEntryFrom.action,
                    StateReactionBehaviour.AfterDefault => actionOnEntry + reactionOnEntryFrom.action,
                    StateReactionBehaviour.BeforeDefault => reactionOnEntryFrom.action + actionOnEntry,
                    _ => throw new NotImplementedException(),
                };
            else
                action = null;

            return hasReactionOnEntryFrom || hasActionOnEntry;
        }

        public void AddActionOnEntry(TState to, Action action)
            => _onEntries[to] = action;

        public void AddReactionOnEntryFrom(Transition<TState, TState> transition, StateReaction reaction)
            => _onEntriesFrom[transition.Reversed] = reaction;

        public void RemoveActionOnEntry(TState to)
            => _onEntries.Remove(to);

        public void RemoveReactionOnEntryFrom(Transition<TState, TState> transition)
            => _onEntriesFrom.Remove(transition.Reversed);

        public void RemoveReactionsOnEntry(TState to)
            => _onEntriesFrom.Remove(to);
    }
}
