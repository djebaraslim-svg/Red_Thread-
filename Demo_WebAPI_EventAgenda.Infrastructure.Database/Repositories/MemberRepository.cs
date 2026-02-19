using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories;


namespace Demo_WebAPI_EventAgenda.Infrastructure.Database.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _DbContext;

        public MemberRepository(AppDbContext dbcontext)
        {
            _DbContext = dbcontext;
        }

        public Member? GetMemberById(long id)
        {
            return _DbContext.Member.SingleOrDefault(m => m.Id == id);
        }

        public Member InsertMember(Member data)
        {
            // Permet d'ajouter dans le contexte
            EntityEntry<Member> element = _DbContext.Member.Add(data);

            // Appliquer la modification du contexte dans la base de données
            _DbContext.SaveChanges();

            // Renvoyé l'élément ajouté à jour
            var result = element.Entity;
            return new Member(result.Id, result.Pseudonym, result.Email, result.AllowNewsLetter, result.Role);
        }

        public string? GetHashPassword(string email)
        {
            return _DbContext.Member.SingleOrDefault(m => m.Email == email)?.Password!;
        }

        public Member GetMemberByEmail(string email)
        {
            Member result =  _DbContext.Member.Single(m => m.Email == email);
            return new Member(result.Id, result.Pseudonym, result.Email, result.AllowNewsLetter, result.Role);
        }
    }
}
