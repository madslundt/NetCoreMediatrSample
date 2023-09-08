using FluentValidation;

namespace Infrastructure.StronglyTypedIds;

public static class StronglyTypedIdValidator
{
    public static void IdMustBeValid<TId, T>(this IRuleBuilder<T, string> ruleBuilder)
        where TId : StronglyTypedIdBaseEntity<TId>
    {
        ruleBuilder.Must((_, id, _) => !string.IsNullOrWhiteSpace(id) &&
                                       ((TId) Activator.CreateInstance(typeof(TId), id)!).IsValid())
            .WithMessage(
                $"Id is not valid. Valid format is {StronglyTypedIdBaseEntity<TId>.GetPlaceholder()}");
    }

    public static void OptionalIdMustBeValid<TId, T>(this IRuleBuilder<T, string?> ruleBuilder)
        where TId : StronglyTypedIdBaseEntity<TId>
    {
        ruleBuilder.Must((_, id, _) =>
                string.IsNullOrWhiteSpace(id) || ((TId) Activator.CreateInstance(typeof(TId), id)!).IsValid())
            .WithMessage(
                $"Id is not valid. Valid format is {StronglyTypedIdBaseEntity<TId>.GetPlaceholder()}");
    }

    public static void IdMustBeValid<TId, T>(this IRuleBuilder<T, TId> ruleBuilder)
        where TId : StronglyTypedIdBaseEntity<TId>
    {
        ruleBuilder.Must((_, id, _) => id.IsValid())
            .WithMessage(
                $"Id is not valid. Valid format is {StronglyTypedIdBaseEntity<TId>.GetPlaceholder()}");
    }

    public static void OptionalIdMustBeValid<TId, T>(this IRuleBuilder<T, TId?> ruleBuilder)
        where TId : StronglyTypedIdBaseEntity<TId>
    {
        ruleBuilder.Must((_, id, _) => id is null || id.IsValid())
            .WithMessage(
                $"Id is not valid. Valid format is {StronglyTypedIdBaseEntity<TId>.GetPlaceholder()}");
    }
}