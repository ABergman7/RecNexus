using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class VenueConfiguration : EntityTypeConfiguration<Venue>
    {
        public VenueConfiguration()
        {
            ToTable("Venue");

            HasKey(x => x.VenueId);

            Property(x => x.VenueId)
                .HasColumnName("VenueID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .IsRequired();

            Property(x => x.Phone)
                .HasColumnName("Phone")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.Address1)
                .HasColumnName("Address1")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.Address2)
                .HasColumnName("Address2")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.City)
                .HasColumnName("City")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.State)
                .HasColumnName("State")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.ZipCode)
                .HasColumnName("ZipCode")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.DateUpdated)
                .HasColumnName("DateUpdated")
                .HasColumnType("datetime")
                .IsOptional();

            Property(x => x.GooglePlaceId)
                .HasColumnName("GooglePlaceID")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();
        }
    }


}