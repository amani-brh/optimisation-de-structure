namespace AmaniRobot.Domain.Exceptions;

internal sealed class InvalidSSNException : DomainException
{
    internal InvalidSSNException(string message)
        : base(message)
    {
    }
}