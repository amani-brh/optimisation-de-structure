namespace AmaniRobot.Domain.ValueObjects;

public sealed class SectionType : IEquatable<SectionType>
{
    public string Value { get; }

    private SectionType() { Value = string.Empty; }

    public SectionType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SectionType cannot be empty.", nameof(value));
        Value = value.Trim();
    }

    public bool Equals(SectionType? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is SectionType s && Equals(s);

    public override int GetHashCode() => HashCode.Combine(Value);

    public override string ToString() => Value;
}
