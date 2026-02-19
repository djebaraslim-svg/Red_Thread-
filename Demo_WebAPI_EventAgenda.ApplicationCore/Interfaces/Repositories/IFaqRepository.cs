using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories
{
        public interface IFaqRepository
        {
            IEnumerable<Faq> Get(bool includesHidden, IEnumerable<string> terms);
            Faq? GetById(long id);
            Faq Insert(Faq data);
            Faq Update(Faq faq);
    }
}
