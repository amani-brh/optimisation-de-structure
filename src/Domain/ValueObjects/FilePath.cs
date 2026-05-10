namespace AmaniRobot.Domain.ValueObjects;

public sealed class FilePath : IEquatable<FilePath>
{
    public string Value { get; }

    private FilePath() { Value = string.Empty; }

    public FilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FilePath cannot be empty.", nameof(value));
        Value = value.Trim();
    }

    public bool Equals(FilePath? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(object? obj) =>
        obj is FilePath f && Equals(f);

    public override int GetHashCode() => HashCode.Combine(Value);

    public override string ToString() => Value;
}