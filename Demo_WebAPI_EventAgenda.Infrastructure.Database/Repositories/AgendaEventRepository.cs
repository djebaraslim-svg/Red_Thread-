using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories;
using Demo_WebAPI_EventAgenda.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Infrastructure.Database.Repositories
{
    public class AgendaEventRepository : IAgendaEventRepository
    {
        // Injection (DI) du DbContext dans le repository (props + ctor)
        private readonly AppDbContext _DbContext;

        public AgendaEventRepository(AppDbContext dbcontext)
        {
            _DbContext = dbcontext;
        }

        public AgendaEvent? GetById(long id)
        {
            return _DbContext.AgendaEvents
                .Include(ae => ae.Category) // Permet d'inclure les données de la table liée EventCategory, via la FK, jointure SQL INNER JOIN
                .SingleOrDefault(ae => ae.Id == id);
        }

        public IEnumerable<AgendaEvent> GetMany(int offset, int limit)
        {
            return _DbContext.AgendaEvents
                .AsNoTracking() // (Optimisation) Ne pas tracker les changements des entités récupérés
                .Skip(offset) // Permet de ne pas prendre les X premiers
                .Take(limit) // Permet de selectionner les Y rows
                .ToList();
        }

        public AgendaEvent Insert(AgendaEvent data)
        {
            //Check de l'existance des catégories 
            EventCategory? categoryInDB = _DbContext.EventCategories.SingleOrDefault(c => c.Name == data.Category.Name);

            //Recréer les données de l'élément à insérer en DB vers la catégorie si elle existe déjà (limitation dû au DDD), sinon laisser les données de la catégorie de l'élément à insérer
            AgendaEvent dataToInsert = new AgendaEvent(
                data.Name,
                data.Desc,
                data.Location,
                data.StartDate,
                data.EndDate,
                categoryInDB ?? data.Category // Si la catégorie existe déjà, on utilise celle de la DB, sinon on utilise celle de l'élément à insérer (qui sera ajouté en même temps que l'élément) avec un coalesce
            );

            // Permet d'ajouter dans le contexte
            EntityEntry<AgendaEvent> element = _DbContext.AgendaEvents.Add(dataToInsert);

            // Appliquer la modification du contexte dans la base de données
            _DbContext.SaveChanges();

            // Renvoyé l'élément ajouté à jour
            return element.Entity;
        }

        public AgendaEvent Update(AgendaEvent data)

        {
            // Permet de marquer l'entité comme modifiée dans le contexte
            EntityEntry<AgendaEvent> result = _DbContext.AgendaEvents.Update(data);

            _DbContext.SaveChanges();

            return result.Entity;
        }

        public bool Delete(long id)
        {

            // AgendaEvent? target = _DbContext.AgendaEvents.SingleOrDefault(ae=>ae.Id == id);
            AgendaEvent? target = GetById(id);

            if (target is null)
            {
                return false;
            }

            //_DbContext.AgendaEvents.Remove(target);
            _DbContext.Remove(target);
            _DbContext.SaveChanges();

            return true;
        }

        public IEnumerable<AgendaEvent> GetByDate(DateTime startDate, DateTime? endDate = null)
        {
            // Cleanup les paramètres d'entrées
            //-> Le début est mis à 0h 00m 00s
            //-> La fin est mise à 23h 59m 59.9999s
           DateTime searchStartDate = startDate.Date; // Permet de ne pas prendre en compte les heures, minutes, secondes, etc... de la date de début
            DateTime searchEndDate = (endDate ?? startDate).Date.AddDays(1).AddTicks(-1); // Permet de ne pas prendre en compte les heures, minutes, secondes, etc... de la date de fin, et d'inclure tous les éléments du jour de la date de fin (en ajoutant 1 jour et en retirant 1 tick)

        
            var result = _DbContext.AgendaEvents
            .AsNoTracking()
            .Where(ae => 
                (  // Si l'event commence avant la recherche, on vérifie que la fin est après le début cherché
                    ae.StartDate <= searchStartDate 
                    && 
                    (ae.EndDate ?? ae.StartDate) >= searchEndDate  // Permet de prendre en compte les éléments qui ont une date de fin nulle (en utilisant la date de début à la place)
                ) 
                ||
                (   // Si l'event termine après la recherche, on vérifie que le début est avant la fin cherchée
                    (ae.EndDate ?? ae.StartDate) >= searchEndDate 
                    &&
                    ae.StartDate <= searchEndDate
                )
                ||
                (
                    //L'event est compris dans la recherche
                    ae.StartDate >= searchStartDate
                    &&           
                    (ae.EndDate ?? ae.StartDate) <= searchEndDate   
                )
                
             ).ToList();

            return result;
        }
    }
}
