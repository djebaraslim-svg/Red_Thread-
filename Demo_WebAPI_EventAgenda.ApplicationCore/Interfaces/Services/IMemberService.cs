using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services
{
    public interface IMemberService
    {
        Member Register(Member member);
        Member Login(string email, string password);

    }
}
