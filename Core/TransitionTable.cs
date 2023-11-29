using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal class TransitionTable<TState, TTrigger>
        where TState : notnull
        where TTrigger : notnull
    {
        private readonly Relation<TState, TTrigger, TState> _transitions = [];
        private readonly Relation<TState, TTrigger, Func<bool>> _conditions = [];

        public bool TryGetTransition(Transition<TState, TTrigger> transition, [MaybeNullWhen(false)] out TState to)
            => _transitions.TryGetValue(transition, out to)
            && (!_conditions.TryGetValue(transition, out var condition)
            || condition.Invoke());

        public void AddTransition(Transition<TState, TTrigger> transition, TState to)
            => _transitions[transition] = to;

        public void AddCondition(Transition<TState, TTrigger> transition, Func<bool> when)
            => _conditions[transition] = when;

        public void RemoveTransition(Transition<TState, TTrigger> transition)
            => _transitions.Remove(transition);

        public void RemoveCondition(Transition<TState, TTrigger> transition)
            => _conditions.Remove(transition);
    }

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
    }

    internal class OnEntryFromTable<TState>
        : Relation<TState, TState, StateReaction>
        where TState : notnull
    {
    }

    internal class OnExitToTable<TState>
        : Relation<TState, TState, StateReaction>
        where TState : notnull
    {
    }

    internal class OnExitTable<TState>
        : Dictionary<TState, Action>
        where TState : notnull
    {
    }

    internal class OnEnterTable<TState>
        : Dictionary<TState, Action>
        where TState : notnull
    {
    }

    internal enum StateReactionBehaviour
    {
        OverrideDefault,
        AfterDefault,
        BeforeDefault,
    }

    internal readonly struct StateReaction(Action action, StateReactionBehaviour behaviour = StateReactionBehaviour.OverrideDefault)
    {
        public readonly Action action = action;
        public readonly StateReactionBehaviour behaviour = behaviour;
    }
}
