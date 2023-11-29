namespace StateMachine.Core
{
    internal readonly struct StateReaction(Action action, StateReactionBehaviour behaviour = StateReactionBehaviour.OverrideDefault)
    {
        public readonly Action action = action;
        public readonly StateReactionBehaviour behaviour = behaviour;
    }
}
