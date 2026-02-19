using Demo_WebAPI_EventAgenda.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Infrastructure.Database.Configs
{
    internal class MemberConfig : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            //Table
            builder.ToTable("Members");

            builder.HasKey(m => m.Id)
                .HasName("PK_Member")
                .IsClustered();

            builder.Property(m => m.Id)
                 .ValueGeneratedOnAdd();

            builder.Property(m => m.Pseudonym)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired(false);

            builder.Property(m => m.Email)
                 .HasMaxLength(320)
                 .IsUnicode()
                 .IsRequired(); 
            
            builder.Property(m => m.Password)
                  .HasColumnName("Password_Hash")
                  .HasMaxLength(200)
                  .IsRequired(); 

            builder.Property(m => m.AllowNewsLetter)
                    .HasColumnName("Allow_Newsletter")
                    .IsRequired();

            builder.Property(m => m.Role)
                    .HasColumnName("Role")
                    .HasMaxLength(50)
                    .IsRequired();

            //Indexe sur l'email 
            builder.HasIndex(m => m.Email)
                .IsUnique()
                .HasDatabaseName("IDX_Members__Email");
        }
    }
}
