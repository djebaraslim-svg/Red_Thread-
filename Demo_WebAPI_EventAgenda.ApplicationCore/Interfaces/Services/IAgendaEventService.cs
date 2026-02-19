using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services
{
    public interface IAgendaEventService
    {
        AgendaEvent GetById(long id);
        IEnumerable<AgendaEvent> GetMany(int page, int nbElement); // Deuxième façon de (int offset, int limit);
        IEnumerable<AgendaEvent> GetAllByDate(DateTime date);
        IEnumerable<AgendaEvent> GetAllByDateRange(DateTime startDate, DateTime endDate);
        
        AgendaEvent Create(AgendaEvent data); //=Insert dans les autres projets
        void UpdateDate(long id, DateTime startDate, DateTime? endDate = null);
        void Delete(long id);
    }
}
