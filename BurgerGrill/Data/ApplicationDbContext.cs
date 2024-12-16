using BurgerGrill.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BurgerGrill.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key and relationship between product and ingredient tables
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            // Seed data
            modelBuilder.Entity<Category>().HasData(

                new Category { CategoryId = 1, Name = "Hamburgers" },
                new Category { CategoryId = 2, Name = "Chicken burgers" },
                new Category { CategoryId = 3, Name = "Sandwiches" },
                new Category { CategoryId = 4, Name = "Sides" },
                new Category { CategoryId = 5, Name = "Drinks" }
            );

            modelBuilder.Entity<Ingredient>().HasData(

                new Ingredient { IngredientId = 1, Name = "Beef" },
                new Ingredient { IngredientId = 2, Name = "Chicken" },
                new Ingredient { IngredientId = 3, Name = "Fish" },
                new Ingredient { IngredientId = 4, Name = "Potatoes" },
                new Ingredient { IngredientId = 5, Name = "Lettuce" },
                new Ingredient { IngredientId = 6, Name = "Tomatoes" },
                new Ingredient { IngredientId = 7, Name = "Pickles" },
                new Ingredient { IngredientId = 8, Name = "Bread" },
                new Ingredient { IngredientId = 9, Name = "Buns" },
                new Ingredient { IngredientId = 10, Name = "Cheese" },
                new Ingredient { IngredientId = 11, Name = "Onions" },
                new Ingredient { IngredientId = 12, Name = "Bacon" }
            );

            modelBuilder.Entity<Product>().HasData(

                //Add hamburger entries here
                new Product
                {
                    ProductId = 1,
                    Name = "Hamburger",
                    Description = "A classic hamburger with a juicy beef patty",
                    Price = 3.49m,
                    Stock = 100,
                    CategoryId = 1
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Cheeseburger",
                    Description = "A flavorful hamburger topped with melted cheese",
                    Price = 3.99m,
                    Stock = 75,
                    CategoryId = 1
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Chicken burger",
                    Description = "A crispy fried chicken patty served in a soft bun",
                    Price = 3.99m,
                    Stock = 70,
                    CategoryId = 2
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Chicken cheeseburger",
                    Description = "A crispy fried chicken patty with melted cheese",
                    Price = 4.49m,
                    Stock = 70,
                    CategoryId = 2
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Tuna sandwich",
                    Description = "A fresh and flavorful tuna salad sandwich",
                    Price = 3.79m,
                    Stock = 50,
                    CategoryId = 3
                },
                new Product
                {
                    ProductId = 6,
                    Name = "French fries",
                    Description = "Crispy, golden french fries made from fresh potatoes",
                    Price = 2.79m,
                    Stock = 350,
                    CategoryId = 4
                },
                new Product
                {
                    ProductId = 7,
                    Name = "Coca-Cola",
                    Description = "A refreshing bottle of Coca-Cola",
                    Price = 2.69m,
                    Stock = 200,
                    CategoryId = 5
                }

            );

            modelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 1 },
                new ProductIngredient { ProductId = 1, IngredientId = 5 },
                new ProductIngredient { ProductId = 1, IngredientId = 6 },
                new ProductIngredient { ProductId = 1, IngredientId = 7 },
                new ProductIngredient { ProductId = 1, IngredientId = 9 },
                new ProductIngredient { ProductId = 1, IngredientId = 11 },
                new ProductIngredient { ProductId = 2, IngredientId = 1 },
                new ProductIngredient { ProductId = 2, IngredientId = 5 },
                new ProductIngredient { ProductId = 2, IngredientId = 6 },
                new ProductIngredient { ProductId = 2, IngredientId = 7 },
                new ProductIngredient { ProductId = 2, IngredientId = 9 },
                new ProductIngredient { ProductId = 2, IngredientId = 10 },
                new ProductIngredient { ProductId = 2, IngredientId = 11 },
                new ProductIngredient { ProductId = 3, IngredientId = 2 },
                new ProductIngredient { ProductId = 3, IngredientId = 5 },
                new ProductIngredient { ProductId = 3, IngredientId = 6 },
                new ProductIngredient { ProductId = 3, IngredientId = 7 },
                new ProductIngredient { ProductId = 3, IngredientId = 9 },
                new ProductIngredient { ProductId = 3, IngredientId = 11 },
                new ProductIngredient { ProductId = 4, IngredientId = 2 },
                new ProductIngredient { ProductId = 4, IngredientId = 5 },
                new ProductIngredient { ProductId = 4, IngredientId = 6 },
                new ProductIngredient { ProductId = 4, IngredientId = 7 },
                new ProductIngredient { ProductId = 4, IngredientId = 9 },
                new ProductIngredient { ProductId = 4, IngredientId = 10 },
                new ProductIngredient { ProductId = 4, IngredientId = 11 },
                new ProductIngredient { ProductId = 5, IngredientId = 3 },
                new ProductIngredient { ProductId = 5, IngredientId = 5 },
                new ProductIngredient { ProductId = 5, IngredientId = 6 },
                new ProductIngredient { ProductId = 5, IngredientId = 8 },
                new ProductIngredient { ProductId = 5, IngredientId = 11 },
                new ProductIngredient { ProductId = 6, IngredientId = 4 }
                );
        }

    }
}
