namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class InvalidStateTransitionException : DomainException
{
    public string CurrentState { get; }
    public string AttemptedState { get; }

    public InvalidStateTransitionException(string currentState, string attemptedState)
        : base($"Cannot transition from {currentState} to {attemptedState}")
    {
        CurrentState = currentState;
        AttemptedState = attemptedState;
    }
}