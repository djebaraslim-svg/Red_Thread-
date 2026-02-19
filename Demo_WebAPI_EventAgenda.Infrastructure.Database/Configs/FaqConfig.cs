using Demo_WebAPI_EventAgenda.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Infrastructure.Database.Configs
{
    internal class FaqConfig : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            // Table
            builder.ToTable("Faq");

            // Clef
            builder.HasKey(m => m.Id)
                .HasName("PK_Faq")
                .IsClustered();

            // Colonnes
            builder.Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Property(f => f.Question)
                .HasMaxLength(255)
                .IsUnicode()
                .IsRequired();

            builder.Property(f => f.Response)
                .HasMaxLength(255)
                .IsUnicode()
                .IsRequired();

            builder.Property(f => f.NbLike)
                .HasColumnName("Nb_Like")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(f => f.IsVisible)
                .HasColumnName("Is_Visible")
                .IsRequired();
        }
    }
}
