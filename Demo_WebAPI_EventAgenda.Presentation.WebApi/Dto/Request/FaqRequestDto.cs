using System.ComponentModel.DataAnnotations;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Request
{
    public class FaqRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public required string Question { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public required string Response { get; set; }
    }

    public class FaqRequestPatchDto
    {
       public required bool? Visibility { get; set; }
        

    }

}

