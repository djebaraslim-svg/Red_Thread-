using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Domain.Models
{
    public class EventCategory
    {
        public long Id { get; private set; }
        public string Name { get; private set; } = default!;

        //Construteur vide besoin pour Entity Framework
        private EventCategory() { }

        //Construteur avec paramètres pour faciliter la création d'instances avec validation
        public EventCategory(string name)
        {
            if(name.Trim().Length < 3)
            {
                throw new ArgumentException("Le nom de la catégorie doit contenir au moins 3 caractères.");
            }
           
            Name = name.Trim();
        }

    }  

}

