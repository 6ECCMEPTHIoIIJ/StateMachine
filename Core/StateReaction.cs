namespace StateMachine.Core
{
    internal readonly struct StateReaction(Action action, StateReactionBehaviours behaviour = StateReactionBehaviours.OverrideDefault)
    {
        public readonly Action action = action;
        public readonly StateReactionBehaviours behaviour = behaviour;
    }
}
