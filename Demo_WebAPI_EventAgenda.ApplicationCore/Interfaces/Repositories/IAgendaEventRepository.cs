using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories
{
    public interface IAgendaEventRepository

    {
        // CRUD Operations
        AgendaEvent? GetById(long id);
        IEnumerable<AgendaEvent> GetMany(int offset, int limit);
        AgendaEvent Insert(AgendaEvent agendaEvent);
        AgendaEvent Update(AgendaEvent agendaEvent);
        bool Delete(long id);

        IEnumerable<AgendaEvent> GetByDate(DateTime startDate, DateTime? endDate = null);
    }
}
