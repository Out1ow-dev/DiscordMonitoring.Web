using DiscordMonitoring.Web.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DiscordMonitoring.Web.Data.Config
{
    public class ServerEntityConfiguration : IEntityTypeConfiguration<ServerEntity>
    {
        public void Configure(EntityTypeBuilder<ServerEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ServerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.ServerDescription)
                .HasMaxLength(500);

            builder.Property(s => s.ServerOnline)
                .HasMaxLength(10);

            builder.Property(s => s.ServerVersion)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.ServerMap)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.ServerIP)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(s => s.QueryPort)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(s => s.GamePort)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(s => s.CreateTime)
                .IsRequired();

            builder.HasOne(s => s.Game)
                .WithMany() 
                .HasForeignKey(s => s.GameId) 
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(s => s.Categories)
                .WithMany();

            builder.HasMany(s => s.OnlineLog)
                .WithOne()
                .HasForeignKey(i => i.ServerId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
