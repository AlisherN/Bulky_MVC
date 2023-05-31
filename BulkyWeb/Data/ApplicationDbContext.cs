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
			optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
		}

		public DbSet<Category> categories { get; set; }

	}
}
