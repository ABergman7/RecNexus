using System.Data.Entity.ModelConfiguration;
using PickUpSports.Models;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Configurations
{
    public class BusinessHoursConfiguration : EntityTypeConfiguration<BusinessHours>
    {
        public BusinessHoursConfiguration()
        {
            ToTable("BusinessHours");

            HasKey(x => x.BusinessHoursId);

            Property(x => x.BusinessHoursId)
                .HasColumnName("BusinessHoursID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.DayOfWeek)
                .HasColumnName("DayOfWeek")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.OpenTime)
                .HasColumnName("OpenTime")
                .HasColumnType("time")
                .IsRequired();

            Property(x => x.CloseTime)
                .HasColumnName("CloseTime")
                .HasColumnType("time")
                .IsRequired();

            Property(x => x.VenueId)
                .HasColumnName("VenueID")
                .HasColumnType("int")
                .IsRequired();
        }
    }

}