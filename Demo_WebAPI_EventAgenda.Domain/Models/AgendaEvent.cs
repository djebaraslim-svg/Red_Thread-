using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Domain.Models
{
    public class AgendaEvent
    {
        public long Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Desc { get; private set; }
        public string? Location { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public EventCategory Category { get; private set; } = default!;

        //Constructeur vide besoin pour Entity Framework
        private AgendaEvent() { }
        
        //Constructeur avec paramètres pour faciliter la création d'instances avec validation
        public AgendaEvent(string name, string? location, string? desc, DateTime startDate, DateTime? endDate, EventCategory category)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Le nom de l'événement doit contenir au moins 3 caractères.",nameof(name));
            }

            if(endDate is not null && endDate < startDate)
            {
                throw new ArgumentException("La date de fin ne peut pas être antérieure à la date de début.");
            }

            Name = name;
            Location = location;
            Desc = desc;
            StartDate = startDate;
            EndDate = endDate;
            Category = category;
        }

        public AgendaEvent ChangeDate(DateTime startDateUpdated, DateTime? endDateUpdated)
        {
            if (endDateUpdated is not null && endDateUpdated < startDateUpdated)
            {
                throw new ArgumentException("La date de fin ne peut pas être antérieure à la date de début.");

                StartDate = startDateUpdated;
                EndDate = endDateUpdated;
            }
                return this; //Retourne l'instance modifiée pour permettre le chaînage de méthodes
        }
    }

}       


