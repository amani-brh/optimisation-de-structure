namespace AmaniRobot.Domain.ValueObjects;

public sealed class Weight : IEquatable<Weight>
{
    private readonly decimal _kg;

    private Weight() { }

    public Weight(decimal kg)
    {
        if (kg < 0)
            throw new ArgumentException("Weight cannot be negative.", nameof(kg));
        _kg = kg;
    }

    public decimal ToDecimal() => _kg;

    public Weight Add(Weight other) => new(_kg + other._kg);

    public bool IsZero() => _kg == 0;

    public bool Equals(Weight? other)
    {
        if (other is null) return false;
        return _kg == other._kg;
    }

    public override bool Equals(object? obj) =>
        obj is Weight w && Equals(w);

    public override int GetHashCode() =>
        HashCode.Combine(_kg);

    public override string ToString() => $"{_kg} kg";
}
