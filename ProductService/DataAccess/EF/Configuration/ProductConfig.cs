using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain;

namespace ProductService.DataAccess.EF.Configuration
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            
            entity.ToTable("Product");
            entity.Property(q => q.Code).IsRequired();
            entity.Property(q => q.Name).IsRequired();            
        }
    }
}
