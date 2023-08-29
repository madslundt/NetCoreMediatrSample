using DataModel.Libs.Extensions;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataModel.Models;

public static class BaseModelExtensions
{
    public static void AddBaseModelContext<T, TId>(this EntityTypeBuilder<T> builder)
        where T : BaseModel<TId> where TId : StronglyTypedIdBaseEntity
    {
        builder.Property(p => p.Id)
            .SupportStronglyTypedId()
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.CreatedUtc)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasKey(k => k.Id);
    }
}