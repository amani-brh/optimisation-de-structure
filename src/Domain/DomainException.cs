namespace AmaniRobot.Domain;

public class DomainException(string businessMessage) : Exception(businessMessage);
