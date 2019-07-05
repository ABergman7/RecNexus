using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class GameStatusConfiguration : EntityTypeConfiguration<GameStatus>
    {
        public GameStatusConfiguration()
        {
            ToTable("GameStatus");

            HasKey(x => x.GameStatusId);

            Property(x => x.GameStatusId)
                .HasColumnName("GameStatusID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Status)
                .HasColumnName("Status")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();
        }
    }

}