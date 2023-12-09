using System.Reflection;

namespace Domain.Services;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<Guid, TEnum> Enumerations = CreateEnumerations();

    protected Enumeration(Guid value, string name)
    {
        Value = value;
        Name = name;
    }

    public Guid Value { get; protected init; }

    public string Name { get; protected init; }

    public static TEnum? FromValue(Guid value)
    {
        return Enumerations.TryGetValue(
            value,
            out var enumeration) ? enumeration
            : default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(q => q.Name == name);
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() &&
               Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    private static Dictionary<Guid, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(q => enumerationType.IsAssignableFrom(q.FieldType))
            .Select(q => (TEnum) q.GetValue(default)!);

        return fieldsForType.ToDictionary(q => q.Value);
    }
}