using Demo_WebAPI_EventAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Utils
{
    public interface IEmailerUtils
    {
        void SendEmail(Member member);
    }
}
