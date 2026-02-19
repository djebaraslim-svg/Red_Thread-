using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions
{
  
        public class FaqException : Exception
        {
            public FaqException(string message) : base(message) { }
        }


        public class FaqNotFoundException : FaqException
        {
            public FaqNotFoundException() : base("FAQ non trouvé !") { }
        }

        public class FaqUpdateException : FaqException
        {
            public FaqUpdateException(string message) : base(message) { }
        }

    }

