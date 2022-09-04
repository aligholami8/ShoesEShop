using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Data;

public class DatabaseContext : IdentityDbContext<Data.User>
{
    public DatabaseContext(DbContextOptions options)
        : base(options)
    {

    }


    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductInOrder> ProductInOrders { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var _passwordHasher = new PasswordHasher<User>();

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(p => p.SubCategories)
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false);

        modelBuilder.Entity<Order>()
            .HasMany(c => c.Products)
            .WithOne(c => c.Order).HasForeignKey(c => c.OrderId);

        modelBuilder.Entity<Product>()
            .HasMany(c => c.Orders)
            .WithOne(c => c.Product).HasForeignKey(c => c.ProductId);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(p => p.Products);

        modelBuilder.Entity<Product>()
            .Property(c => c.SubPicture)
            .HasConversion(
                c => JsonConvert.SerializeObject(c),
                e => JsonConvert.DeserializeObject<List<string>>(e)!);

        modelBuilder.Entity<Product>()
            .Property(c => c.Sizes)
            .HasConversion(
                c => JsonConvert.SerializeObject(c),
                e => JsonConvert.DeserializeObject<List<string>>(e)!);

        var adminRole = new IdentityRole()
            { Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString(), Name = "admin", NormalizedName = "ADMIN", };

        modelBuilder.Entity<IdentityRole>().HasData(adminRole);

        var adminUser = new User() { Id = Guid.NewGuid().ToString(), FullName = "ادمین", UserName = "admin", NormalizedUserName = "ADMIN" };
        adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "123456789");
        modelBuilder.Entity<User>().HasData(adminUser);

        var adminUserRole = new IdentityUserRole<string>() { RoleId = adminRole.Id, UserId = adminUser.Id };

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);

    }
}