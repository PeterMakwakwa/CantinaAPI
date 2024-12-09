using CantinaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CantinaAPI.Data
{
    public class CantinaDbContext : IdentityDbContext<UserModel>
    {

        public CantinaDbContext(DbContextOptions<CantinaDbContext> options) : base(options) { }
        public DbSet<MenuItemModel> Items { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure Review -> Item relationship
            builder.Entity<ReviewModel>()
                .HasOne(r => r.Item) // Review has one Item
                .WithMany(i => i.Reviews) // Item has many Reviews
                .HasForeignKey(r => r.ItemId) // Foreign Key
                .OnDelete(DeleteBehavior.Cascade); // Optionally set delete behavior

            // Configure Review -> User relationship
            builder.Entity<ReviewModel>()
                .HasOne(r => r.User) // Review has one User
                .WithMany() // User can have many Reviews (optional)
                .HasForeignKey(r => r.UserId) // Foreign Key for User
                .OnDelete(DeleteBehavior.Restrict); // You can choose other delete behaviors here if needed

            // Ensure that foreign keys are correctly applied and no shadow state is generated
            builder.Entity<ReviewModel>()
                .HasKey(r => r.Id); 



            // Seed Items
            builder.Entity<MenuItemModel>().HasData(
                new MenuItemModel { Id = 1, Name = "Pizza", Description = "Cheesy Pizza", Price = 9.99M, Type = "Dish",ImageUrl= "http://example.com/images/pizza.jpg" },
                new MenuItemModel { Id = 2, Name = "Coke", Description = "Refreshing drink", Price = 1.99M, Type = "Drink", ImageUrl= "http://example.com/images/coke.jpg" }
            );
            builder.Entity<MenuItemModel>()
               .HasIndex(i => i.Name)
               .IsUnique(); 

            // Seed Users
            var user1Id = "user1-id";
            var user2Id = "user2-id";

            builder.Entity<UserModel>().HasData(
                new UserModel
                {
                    Id = user1Id,
                    UserName = "johndoe",
                    Email = "johndoe@example.com",
                    FullName = "John Doe",
                    NormalizedUserName = "JOHNDOE",
                    NormalizedEmail = "JOHNDOE@EXAMPLE.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAEGzR29S+clNpeKQiQZosBmIgjexns+nHo2uRSqBYDKSH11+E=", // Pre-hashed password
                },
                new UserModel
                {
                    Id = user2Id,
                    UserName = "janedoe",
                    Email = "janedoe@example.com",
                    FullName = "Jane Doe",
                    NormalizedUserName = "JANEDOE",
                    NormalizedEmail = "JANEDOE@EXAMPLE.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAEGzR29S+clNpeKQiQZosBmIgjexns+nHo2uRSqBYDKSH11+E=", // Pre-hashed password
                }
            );

            // Seed Reviews
            builder.Entity<ReviewModel>().HasData(
                new ReviewModel
                {
                    Id=1,
                    UserId = user1Id,
                    ItemId = 1, // Pizza
                    Comment = "Delicious and cheesy!",
                    Rating = 5
                },
                new ReviewModel
                {
                    Id=2,
                    UserId = user2Id,
                    ItemId = 2, // Coke
                    Comment = "Very refreshing and chilled.",
                    Rating = 4
                },
                new ReviewModel
                {
                    Id = 3,
                    UserId = user1Id,
                    ItemId = 2, // Coke
                    Comment = "Too sweet for my taste.",
                    Rating = 3
                }
            );


            var readerRoleId = "3be1d968-869b-4364-b28e-fe822c5fe01d";
            var writerRoleId = "01f90958-9db8-4564-be54-5e3e736532d8";
            base.OnModelCreating(builder);
            var role = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=readerRoleId,
                    ConcurrencyStamp= readerRoleId,
                    Name ="Reader",
                    NormalizedName="Reader".ToUpper(),
                },
                 new IdentityRole
                {
                    Id=writerRoleId,
                    ConcurrencyStamp= writerRoleId,
                    Name ="Writer",
                    NormalizedName="Writer".ToUpper(),
                }
            };

            builder.Entity<IdentityRole>().HasData(role);
        }
    }
}
