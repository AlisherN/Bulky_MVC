using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
	public class ApplicationDbContext : DbContext
	{
		protected readonly IConfiguration _configuration;
		public ApplicationDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_configuration.GetConnectionString("BulkyWebDatabase"));
		}

		public DbSet<Category> categories { get; set; }

		// Seeder qilib databasega test ma'lumot qo'shish
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(
				new Category { Id=1, Name = "Action", DisplayOrder = 1},
				new Category { Id=2, Name = "SciFi", DisplayOrder = 2},
				new Category { Id=3, Name = "History", DisplayOrder = 3}
				);
		}
	}
}
