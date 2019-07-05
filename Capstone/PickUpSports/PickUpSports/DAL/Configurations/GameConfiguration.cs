using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            ToTable("Game");

            HasKey(x => x.GameId);

            Property(x => x.GameId)
                .HasColumnName("GameID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.StartTime)
                .HasColumnName("StartTime")
                .HasColumnType("datetime")
                .IsRequired();

            Property(x => x.EndTime)
                .HasColumnName("EndTime")
                .HasColumnType("datetime")
                .IsRequired();

            Property(x => x.VenueId)
                .HasColumnName("VenueID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.SportId)
                .HasColumnName("SportID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.ContactId)
                .HasColumnName("ContactID")
                .HasColumnType("int")
                .IsOptional();

            Property(x => x.GameStatusId)
                .HasColumnName("GameStatusID")
                .HasColumnType("int")
                .IsRequired();
        }
    }
}