using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories;
using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services;
using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Utils;
using Demo_WebAPI_EventAgenda.Domain.Models;
using Konscious.Security.Cryptography;
using Soenneker.Hashing.Argon2;
using System;
using System.Collections.Generic;
using System.Text;
using static Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions.MemberException;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEmailerUtils _emailerUtils;

        public MemberService(IMemberRepository memberRepository, IEmailerUtils emailerUtils)
        {
            _memberRepository = memberRepository;
            _emailerUtils = emailerUtils;
        }

        public Member Login(string email, string password)
        {
           string? hashPwd = _memberRepository.GetHashPassword(email);

            if (hashPwd is null)
            {
                // Si le compte n'existe pas, on a crée une exception personnalisée pour indiquer que les identifiants sont incorrects, sans préciser si c'est l'email ou le mot de passe qui est incorrect pour des raisons de sécurité.
                throw new MemberBadCredentialException();
            }

            bool isPasswordValid = Argon2HashingUtil.Verify(password, hashPwd).Result;
            if (!isPasswordValid)
            {
                // Si le mot de passe est incorrect, on a crée une exception personnalisée pour indiquer que les identifiants sont incorrects, sans préciser si c'est l'email ou le mot de passe qui est incorrect pour des raisons de sécurité.
                throw new MemberBadCredentialException();
            }

            return _memberRepository.GetMemberByEmail(email);
        }

        public Member Register(Member member)
        {
            //Verification des données d'inscription, si le pseudonyme est vide ou null, si le mot de passe est vide ou null
            if (string.IsNullOrEmpty(member.Pseudonym))
            {
                throw new ArgumentNullException("Le pseudonyme est requis pour l'inscription.");
            }
            if (string.IsNullOrEmpty(member.Password))
            {
                throw new ArgumentNullException("Le mot de passe est requis pour l'inscription.");
            }
            
            //Hashage du mot de passe (Hashage = mot de passe irréversible à sens unique, cryptage = réversible -> échange de clés de cryptage)
            // Le .Result est nécessaire pour attendre le résultat de la tâche asynchrone de hashage, car Hash est une méthode asynchrone qui retourne une Task<string>.
            string hashPwd = Argon2HashingUtil.Hash(member.Password).Result;

            // Remplace le mot de passe en clair par le mot de passe hashé -> recréation de l'objet Member avec le mot de passe hashé
            Member memberToInsert = new Member(
                member.Pseudonym,
                member.Email,
                member.AllowNewsLetter,
                hashPwd   
            );

            // Créer le compte du membre dans la base de données via le repository
            Member memberInserted = _memberRepository.InsertMember(memberToInsert);

            // Envoyer un email de bienvenue au membre via l'utilitaire d'emailing
            _emailerUtils.SendEmail(member);

            // Envoi du compte créé en base de données pour confirmation à l'appelant
            return memberInserted;
        }
    }
}
