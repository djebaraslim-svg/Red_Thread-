using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions
{
    //Erreur personnalisée pour les AgendaEvent

    //-Base pour les erreurs liées aux règles métier des AgendaEvent
        public class AgendaEventException : Exception
        {
        // Donnée de l'event (pas d'obligation)
            public AgendaEvent? AgendaEventData { get; set; }

        // Constructeur de l'exception avec message et donnée optionnelle = erreur générique pour les événements d'agenda
        public AgendaEventException(string message, AgendaEvent? data = null) : base(message)
            {
            AgendaEventData = data;
            }

        //-Spécialiser l'exception pour les erreurs lors de la création d'un événement
        public class AgendaEventCreateException : AgendaEventException
        {
            private const string INNER_MESSAGE = "Erreur lors de la création de l'événement";
            public AgendaEventCreateException(AgendaEvent data) : base(INNER_MESSAGE, data)
            { }
            public AgendaEventCreateException(string message, AgendaEvent data) : base($"{INNER_MESSAGE} : {message}", data)
            { }

        }

        //-Erreur spécialisée quand un événement n'est pas trouvé (ex: lors de la suppression ou de la récupération d'un événement par id)
        public class AgendaEventNotFoundException : AgendaEventException
        {
            public AgendaEventNotFoundException() : base("Événement non trouvé!") // C'est le not found
            { }
        }
    }
}
