using System;

namespace StateMachine.Core
{
    internal readonly struct StateReaction
    {
        public required Action Action { get; init; }
        public required StateReactionBehaviours Behaviour { get; init; }
    }
}
