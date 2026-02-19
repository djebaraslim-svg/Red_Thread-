using Demo_WebAPI_EventAgenda.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Infrastructure.Database.Configs
{
    internal class EventCategoryConfig : IEntityTypeConfiguration<EventCategory>
    {
        public void Configure(EntityTypeBuilder<EventCategory> builder)
        {
            //Table
            builder.ToTable("Event_Categories");

            //Clef
            builder.HasKey(ec => ec.Id)
                .HasName("PK_Event_Categories_Id")
                .IsClustered();

            //Colonnes
            builder.Property(ec => ec.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ec => ec.Name)
                .HasMaxLength(50)
                .IsRequired();

            //Index
            builder.HasIndex(ec => ec.Name)
                .IsUnique()
                .HasDatabaseName("IDX_Event_Categories__Name");

        }
    }
}
