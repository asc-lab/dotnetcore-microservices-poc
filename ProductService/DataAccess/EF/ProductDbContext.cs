using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess.EF.Configuration;
using ProductService.Domain;

namespace ProductService.DataAccess.EF;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Cover> Covers { get; set; }
    public DbSet<Choice> Choices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfig());
        modelBuilder.ApplyConfiguration(new QuestionConfig());
        modelBuilder.ApplyConfiguration(new ChoiceQuestionConfig());
        modelBuilder.ApplyConfiguration(new ChoiceConfig());
    }
}