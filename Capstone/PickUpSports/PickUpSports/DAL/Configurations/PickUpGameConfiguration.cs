using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class PickUpGameConfiguration : EntityTypeConfiguration<PickUpGame>
    {
        public PickUpGameConfiguration()
        {
            ToTable("PickUpGame");

            HasKey(x => x.PickUpGameId);

            Property(x => x.PickUpGameId)
                .HasColumnName("PickUpGameID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.GameId)
                .HasColumnName("GameID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.ContactId)
                .HasColumnName("ContactID")
                .HasColumnType("int")
                .IsOptional();
        }
    }
}