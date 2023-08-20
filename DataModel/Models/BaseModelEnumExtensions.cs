using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataModel.Models;

public static class BaseModelEnumExtensions
{
    public static void AddBaseModelEnumExtensions<T, TEnum>(this EntityTypeBuilder<T> builder)
        where T : BaseModelEnum<TEnum>
        where TEnum : Enum
    {
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired();

        builder.HasIndex(k => k.Name)
            .IsUnique();

        builder.HasKey(k => k.Id);

        builder.HasData(Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
            .Select(baseModelEnum => (T) Activator.CreateInstance(typeof(T), baseModelEnum)!).ToArray());
    }
}