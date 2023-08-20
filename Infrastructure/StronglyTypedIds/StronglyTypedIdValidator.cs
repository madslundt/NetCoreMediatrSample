using FluentValidation;

namespace Infrastructure.StronglyTypedIds;

public static class StronglyTypedIdValidator
{
    public static IRuleBuilderOptions<T, string> IdMustBeValid<TId, T>(
        this IRuleBuilder<T, string> ruleBuilder) where TId : StronglyTypedIdBaseEntity
    {
        return ruleBuilder.Must((_, id, _) => ((TId) Activator.CreateInstance(typeof(TId), id)!).IsValid())
            .WithMessage(
                $"Id is not valid. Id must have the format {((TId) Activator.CreateInstance(typeof(TId), "")!).GetPlaceholder()}");
    }

    public static IRuleBuilderOptions<T, string?> OptionalIdMustBeValid<TId, T>(
        this IRuleBuilder<T, string?> ruleBuilder) where TId : StronglyTypedIdBaseEntity?
    {
        return ruleBuilder.Must((_, id, _) =>
                string.IsNullOrEmpty(id) || ((TId) Activator.CreateInstance(typeof(TId), id)!).IsValid())
            .WithMessage(
                $"Id is not valid. Id must have the format {((TId) Activator.CreateInstance(typeof(TId), "")!).GetPlaceholder()}");
    }
}