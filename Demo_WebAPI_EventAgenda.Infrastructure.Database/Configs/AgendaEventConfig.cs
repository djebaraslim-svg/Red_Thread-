using Demo_WebAPI_EventAgenda.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Infrastructure.Database.Configs
{
    internal class AgendaEventConfig : IEntityTypeConfiguration<AgendaEvent>
    {
        public void Configure(EntityTypeBuilder<AgendaEvent> builder)
        {
            //Table
            builder.ToTable("Agenda_Events");

            //Clef
            builder.HasKey(ae => ae.Id)
                .HasName("PK_Agenda_Events_Id")
                .IsClustered();

            //Colonnes : on va sur chaque propriété pour définir les contraintes
            builder.Property(ae => ae.Id)
               .ValueGeneratedOnAdd();

            builder.Property(ae => ae.Name)
                .HasMaxLength(100)
                .IsUnicode() //NVARCHAR en base
                .IsRequired();

            builder.Property(ae => ae.Desc)
                .HasMaxLength(2000)
                .IsUnicode()
                .IsRequired(false); //nullable

            builder.Property(ae => ae.Location)
                .HasMaxLength(100)
                .IsUnicode() 
                .IsRequired(false);

            builder.Property(ae => ae.StartDate)
                .HasColumnName("Start_Date")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(ae => ae.EndDate)
                .HasColumnName("End_Date")
                .HasColumnType("datetime2")
                .IsRequired(false);

            //Index : on veut un index unique sur la combinaison Nom + Lieu + Date de début. Cela évite d'avoir 2 événements identiques. 
            builder.HasIndex(ae => new {ae.Name, ae.Location, ae.StartDate})
                .IsUnique()
                .HasDatabaseName("IDX_Agenda_Events__Name_Loc_Date");

            //Relations : clé étrangère vers EventCategory
            builder.HasOne(ae => ae.Category) //Un AgendaEvent a une EventCategory. Lien de navigation de AgentEvent vers EventCategory
                .WithMany() //Une EventCategory peut être liée à plusieurs AgendaEvent. Pas de lien de navigation de EventCategory vers AgendaEvent
                .HasForeignKey("CategoryId") //La clé étrangère en base de données s'appelle CategoryId. Elle peut être auto générée si on ne la précise pas.
                .HasConstraintName("FK_Agenda_Events__Categories")
                .IsRequired(); //La catégorie est obligatoire


  
        }
    }
}
