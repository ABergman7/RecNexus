using PickUpSports.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace PickUpSports.DAL.Configurations
{
    public class SportPreferenceConfiguration : EntityTypeConfiguration<SportPreference>
    {
        public SportPreferenceConfiguration()
        {
            ToTable("SportPreference");
            // Change as needed for primary key
            HasKey(x => x.SportPrefID);
            // Configure any foreign keys

            Property(x => x.SportPrefID)
                .HasColumnName("SportPrefID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.SportID)
                .HasColumnName("SportID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.ContactID)
                .HasColumnName("ContactID")
                .HasColumnType("int")
                .IsRequired();

        }
    }
}