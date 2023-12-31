﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StateMachine.Core
{
    internal abstract class ActionTable<TState>
        where TState : struct
    {
        private readonly Dictionary<TState, Action> _actions = [];
        private readonly Relation<TState, TState, StateReaction> _reactions = [];

        public void AddState(TState state)
        {
            _reactions[state] = [];
        }

        public void AddAction(TState state, Action action)
            => _actions[state] = action;

        public void AddReaction(TState state, TState on, StateReaction reaction)
            => _reactions[state, on] = reaction;

        public bool TryGetAction(TState state, TState on, [MaybeNullWhen(false)] out Action action)
        {
            if (_reactions.TryGetValue(state, on, out var reaction))
            {
                action = !_actions.TryGetValue(state, out var mainAction)
                    ? reaction.Action
                    : reaction.Behaviour switch
                    {
                        StateReactionBehaviours.OverrideDefault => reaction.Action,
                        StateReactionBehaviours.AfterDefault => mainAction + reaction.Action,
                        StateReactionBehaviours.BeforeDefault => reaction.Action + mainAction,
                        _ => throw new ArgumentOutOfRangeException($"Given value {reaction} is invalid for {nameof(StateReaction)}"),
                    };

                return true;
            }

            return _actions.TryGetValue(state, out action);
        }

        public bool TryGetAction(TState state, [MaybeNullWhen(false)] out Action action)
            => _actions.TryGetValue(state, out action);
    }
}