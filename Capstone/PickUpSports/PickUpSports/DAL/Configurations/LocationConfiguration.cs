using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        public LocationConfiguration()
        {
            ToTable("Location");

            HasKey(x => x.LocationId);

            Property(x => x.LocationId)
                .HasColumnName("LocationID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Latitude)
                .HasColumnName("Latitude")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.Longitude)
                .HasColumnName("Longitude")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.VenueId)
                .HasColumnName("VenueID")
                .HasColumnType("int")
                .IsRequired();
        }
    }

}