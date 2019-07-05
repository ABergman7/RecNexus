using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            ToTable("Friend");
            // Change as needed for primary key
            HasKey(x => x.FriendID);
            // Configure any foreign keys

            Property(x => x.FriendID)
                .HasColumnName("FriendID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.ContactID)
                .HasColumnName("ContactID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.FriendContactID)
                .HasColumnName("FriendContactID")
                .HasColumnType("int")
                .IsRequired();

        }
    }


}