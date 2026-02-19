using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        string? GetHashPassword ( string email);
        Member GetMemberByEmail(string email);
        Member InsertMember (Member data);

    }
}
