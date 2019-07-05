using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class VenueOwnerConfiguration : EntityTypeConfiguration<VenueOwner>
    {
        public VenueOwnerConfiguration()
        {
            ToTable("VenueOwner");
            // Change as needed for primary key
            HasKey(x => x.VenueOwnerId);
            // Configure any foreign keys

            Property(x => x.VenueOwnerId)
                .HasColumnName("VenueOwnerID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.LastName)
                .HasColumnName("LastName")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.Phone)
                .HasColumnName("Phone")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.CompanyName)
                .HasColumnName("CompanyName")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.SignUpDate)
                .HasColumnName("SignUpDate")
                .HasColumnType("datetime")
                .IsRequired();

            Property(x => x.VenueId)
                .HasColumnName("VenueID")
                .HasColumnType("int")
                .IsRequired();

        }
    }

}