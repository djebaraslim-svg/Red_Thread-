using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories;
using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services;
using Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions;
using Demo_WebAPI_EventAgenda.Domain.Models;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Services
{
    public class FaqService : IFaqService
    {
        private readonly IFaqRepository _faqRepository;

        public FaqService(IFaqRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }


        public IEnumerable<Faq> GetAll(bool includesHidden = false)
        {
            return _faqRepository.Get(includesHidden, []);
        }

        public IEnumerable<Faq> GetBySearch(IEnumerable<string> terms, bool includesHidden = false)
        {
            if (terms.Count() == 0)
            {
                throw new ArgumentOutOfRangeException("La recherche doit contenir au moins un terme");
            }

            return _faqRepository.Get(includesHidden, terms);
        }

        public Faq Create(Faq data)
        {
            return _faqRepository.Insert(data);
        }

        public void UpdateVisibility(long id, bool visibility)
        {
            Faq? faq = _faqRepository.GetById(id);

            if (faq is null)
                throw new FaqNotFoundException();

            if (faq.IsVisible == visibility)
                throw new FaqUpdateException($"La FAQ est déjà {(visibility ? "visible" : "masqué")}");

            faq.ChangeVisibility(visibility);
            _faqRepository.Update(faq);
        }

        public void AddLike(long id)
        {
            Faq? faq = _faqRepository.GetById(id);

            if (faq is null)
                throw new FaqNotFoundException();

            faq.IncrLike();
            _faqRepository.Update(faq);
        }
    }
}
