using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class ReviewConfiguration : EntityTypeConfiguration<Review>
    {
        public ReviewConfiguration()
        {
            ToTable("Review");

            HasKey(x => x.ReviewId);

            // Configure any foreign keys

            Property(x => x.ReviewId)
                .HasColumnName("ReviewID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Timestamp)
                .HasColumnName("Timestamp")
                .HasColumnType("datetime")
                .IsRequired();

            Property(x => x.Rating)
                .HasColumnName("Rating")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Comments)
                .HasColumnName("Comments")
                .HasColumnType("nvarchar")
                .IsOptional();

            Property(x => x.IsGoogleReview)
                .HasColumnName("IsGoogleReview")
                .HasColumnType("bit")
                .IsRequired();

            Property(x => x.GoogleAuthor)
                .HasColumnName("GoogleAuthor")
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(x => x.ContactId)
                .HasColumnName("ContactID")
                .HasColumnType("int")
                .IsOptional();

            Property(x => x.VenueId)
                .HasColumnName("VenueID")
                .HasColumnType("int")
                .IsRequired();

        }
    }

}