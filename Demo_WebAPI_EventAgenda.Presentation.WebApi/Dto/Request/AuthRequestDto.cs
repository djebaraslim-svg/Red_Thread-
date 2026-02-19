using System.ComponentModel.DataAnnotations;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Request
{
    public class AuthRegisterRequestDto
    {
        [MinLength(5)]
        [MaxLength(50)]             
        public required String? Pseudonym { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        public required String Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression("(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).*")]
        public required  String Password { get; set; }

        [Required]
        public required bool AllowNewsLetter { get; set; }
    }

    public class AuthLoginRequestDto
    {
        [Required]
        public required String Email { get; set; }

        [Required]
        public required String Password { get; set; }

    }
}
