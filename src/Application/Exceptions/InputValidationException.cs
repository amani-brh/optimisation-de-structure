using AmaniRobot.Domain;

namespace AmaniRobot.Application.Exceptions;

public sealed class InputValidationException(string message) : DomainException(message);
