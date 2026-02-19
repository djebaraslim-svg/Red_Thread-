using Demo_WebAPI_EventAgenda.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;

namespace Demo_WebAPI_EventAgenda.Domain.Models
{
    public class Member
    {
        public long Id { get; private set; }
        public string? Pseudonym { get; private set; }
        public string Email { get; private set; } = default!;
        public string? Password { get; private set; }
        public bool AllowNewsLetter {  get; private set; }
        public Role Role { get; private set; }
    
        //Constructeur vide besoin pour Entity Framework
        private Member() { }

        //Construteur avec paramètres pour faciliter la création d'instances avec validation
        public Member( string? pseudonym, string email, bool allowNewsLetter, string? password = null)
        {
            if (pseudonym is not null && (pseudonym.Trim().Length < 3 || pseudonym.Trim().Length > 50))
            {
                throw new ArgumentException("Le pseudonyme n'est pas valide. Vous devez entrer un pseudonyme entre 3 et 50 caractères .", nameof(pseudonym));
            }

            if (string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email, out _))
            {
                throw new ArgumentException("Vous devez entrer une adresse email valide.", nameof (email));
            }

            Pseudonym = pseudonym;
            Email = email;
            Password = password;
            AllowNewsLetter = allowNewsLetter;
            Role = Role.Peon; // Par défaut, tous les nouveaux membres sont des Peons          
        }

        public Member(long id, string? pseudonym, string email, bool allowNewsLetter, Role role, string? password = null) : this(pseudonym, email, allowNewsLetter, password)
        {
            Id = id;
        }
    }
}
