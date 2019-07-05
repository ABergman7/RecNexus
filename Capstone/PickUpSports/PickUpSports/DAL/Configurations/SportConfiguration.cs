using PickUpSports.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PickUpSports.DAL.Configurations
{
    public class SportConfiguration : EntityTypeConfiguration<Sport>
    {
        public SportConfiguration()
        {
            ToTable("Sport");
            // Change as needed for primary key
            HasKey(x => x.SportID);
            // Configure any foreign keys

            Property(x => x.SportID)
                .HasColumnName("SportID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.SportName)
                .HasColumnName("SportName")
                .HasColumnType("nvarchar")
                .IsRequired();

        }
    }
}