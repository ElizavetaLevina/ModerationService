using Microsoft.EntityFrameworkCore;
using ModerationService.DAL.Configurations;

namespace ModerationService.DAL.Models
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<ModerationResultEntity> ModerationResults { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new ModerationResultConfiduration());
		}
	}
}
