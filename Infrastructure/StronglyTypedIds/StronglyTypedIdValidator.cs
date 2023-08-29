using FluentValidation;

namespace Infrastructure.StronglyTypedIds;

public static class StronglyTypedIdValidator
{
    public static void IdMustBeValid<TId, T>(this IRuleBuilder<T, string> ruleBuilder)
        where TId : StronglyTypedIdBaseEntity
    {
        ruleBuilder.Must((_, id, _) => !string.IsNullOrWhiteSpace(id) &&
                                       ((TId) Activator.CreateInstance(typeof(TId), id)!).IsValid())
            .WithMessage(
                $"Id is not valid");
    }

    public static void OptionalIdMustBeValid<TId, T>(this IRuleBuilder<T, string?> ruleBuilder)
        where TId : StronglyTypedIdBaseEntity?
    {
        ruleBuilder.Must((_, id, _) =>
                string.IsNullOrWhiteSpace(id) || ((TId) Activator.CreateInstance(typeof(TId), id)!).IsValid())
            .WithMessage(
                $"Id is not valid");
    }
}