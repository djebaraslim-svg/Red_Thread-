using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services
{
        public interface IFaqService
        {
            public Faq Create(Faq data);
            public IEnumerable<Faq> GetAll(bool includesHidden = false);
            public IEnumerable<Faq> GetBySearch(IEnumerable<string> terms, bool includesHidden = false);
            public void UpdateVisibility(long id, bool visibility);
            public void AddLike(long id);
    }
}
