using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModerationService.DAL.Models;

namespace ModerationService.DAL.Configurations
{
	public class ModerationResultConfiduration : IEntityTypeConfiguration<ModerationResultEntity>
	{
		public void Configure(EntityTypeBuilder<ModerationResultEntity> builder)
		{
			builder.HasKey(c => c.Id);

			builder.Property(c => c.DateModerate).HasColumnType("timestamp without time zone");
		}
	}
}
