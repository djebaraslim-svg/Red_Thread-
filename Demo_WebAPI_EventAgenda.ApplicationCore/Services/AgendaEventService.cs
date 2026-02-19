using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories;
using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services;
using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions.AgendaEventException;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Services
{
    public class AgendaEventService : IAgendaEventService
    {
        private IAgendaEventRepository _agendaEventRepository;

        public AgendaEventService(IAgendaEventRepository agendaEventRepository)
        {
            _agendaEventRepository = agendaEventRepository;
        }

        public AgendaEvent Create(AgendaEvent data)
        {
            //ERREUR : Si la date du début de l'event est plus petite que la date de demain, on ne peut pas créer l'event
            if (data.StartDate < DateTime.Today.AddDays(1))
            {
                //Génerer une erreur liée aux règles métier 
                throw new AgendaEventCreateException(data);
            }

            return _agendaEventRepository.Insert(data);
        }

        public void Delete(long id)
        {
            bool success = _agendaEventRepository.Delete(id);

            if (!success)
            {
                throw new AgendaEventNotFoundException();
            }

        }

        public IEnumerable<AgendaEvent> GetAllByDate(DateTime date)
        {
            return _agendaEventRepository.GetByDate(date);
        }

        public IEnumerable<AgendaEvent> GetAllByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentOutOfRangeException("Les dates sont invalides");
            }

            return _agendaEventRepository.GetByDate(startDate, endDate);
        }

        public AgendaEvent GetById(long id)
        {
            AgendaEvent? data = _agendaEventRepository.GetById(id);
            
            if (data is null)
            {
                throw new AgendaEventNotFoundException();
            }
            return data;

            //Alternative plus concise avec l'opérateur de coalescence null (nouveau depuis un an et demi) :
            //return _agendaEventRepository.GetById(id) ?? throw new AgendaEventNotFoundException();
        }
       
        public IEnumerable<AgendaEvent> GetMany(int page, int nbElement) // Récupérer les éléments par page
        {
            if(page <= 0 || nbElement <= 0)
            {                 
                throw new ArgumentOutOfRangeException("La page ou le nombre d'éléments selectionnés doit être supérieur à zéro.");
            }

            int offset = (page - 1) * nbElement;
            int limit = nbElement; 

           return _agendaEventRepository.GetMany(offset, limit);
        }

        public void UpdateDate(long id, DateTime startDate, DateTime? endDate)
        {
            // Récupérer l'élément à mettre à jour : l'event de l'agenda
            AgendaEvent? agendaEvent = _agendaEventRepository.GetById(id);

            if (agendaEvent is null)
            {
                throw new AgendaEventNotFoundException();
            }

            // Mettre à jour les dates de l'élément, en utilisant la méthode ChangeDate de l'entité AgendaEvent, modification des données via le Domain Model (pattern DDD)
            agendaEvent.ChangeDate(startDate, endDate);

            // Recupération du changement dans la BDD via le repository
            _agendaEventRepository.Update(agendaEvent);

        }
            
    }
}
