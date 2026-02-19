using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Demo_WebAPI_EventAgenda.Domain.Models;
using Demo_WebAPI_EventAgenda.Infrastructure.Database.Configs;
using Microsoft.EntityFrameworkCore;

namespace Demo_WebAPI_EventAgenda.Infrastructure.Database
{
    // Configuration de la base de données avec Entity Framework Core
    public class AppDbContext : DbContext
    {
        // Configuration des tables de la base de données
        public DbSet<AgendaEvent> AgendaEvents { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }

        public DbSet<Member> Member { get; set; }

        public DbSet<Faq> Faq { get; set; }

        // Definition du constructeur qu sera utiliser par l'injection de dépendance : 
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        //  Configuration de la base de données 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Chargement des classes de configuration pour chaque entité qui implémente IEntityTypeConfiguration<T>
            // Cela permet de séparer la configuration des entités dans des classes dédiées
            // Ajout manuel des configurations spécifiques
            /*
            modelBuilder.ApplyConfiguration(new AgendaEventConfig());
            modelBuilder.ApplyConfiguration(new EventCategoryConfig());
            */

            // Ou chargement automatique de toutes les configurations dans l'assembly courant qui implémentent IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}
