using PickUpSports.Models;
using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class TimePreferenceConfiguration : EntityTypeConfiguration<TimePreference>
    {
        public TimePreferenceConfiguration()
        {
            ToTable("TimePreference");
            // Change as needed for primary key
            HasKey(x => x.TimePrefID);
            // Configure any foreign keys

            Property(x => x.TimePrefID)
                .HasColumnName("TimePrefID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.DayOfWeek)
                .HasColumnName("DayOfWeek")
                .HasColumnType("int")
                .IsOptional();

            Property(x => x.BeginTime)
                .HasColumnName("BeginTime")
                .HasColumnType("time")
                .IsOptional();

            Property(x => x.EndTime)
                .HasColumnName("EndTime")
                .HasColumnType("time")
                .IsOptional();

            Property(x => x.ContactID)
                .HasColumnName("ContactID")
                .HasColumnType("int")
                .IsRequired();

        }
    }

}