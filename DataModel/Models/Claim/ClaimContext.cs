using Microsoft.EntityFrameworkCore;

namespace DataModel.Models
{
    public class ClaimContext
    {
        public static void Build(ModelBuilder builder)
        {
            builder.Entity<Claim.Claim>(b =>
            {
                b.HasKey(k => k.Id);
                b.ToTable("Claims");
            });
        }
    }
}
