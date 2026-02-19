using System.ComponentModel.DataAnnotations;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Request
{
    public class AgendaEventRequestDto
    {
        [Required(AllowEmptyStrings = false), MinLength(3), MaxLength(50)] // Validation des formats des données entrantes pour le champ Name (data annotations)
        public required string Name { get; set; }

        [MinLength(10), MaxLength(2000)] // Validation des formats des données entrantes pour le champ Desc (data annotations)
        public required string? Desc { get; set; }

        [MaxLength(50)] // Validation des formats des données entrantes pour le champ Location (data annotations)
        public required string? Location { get; set; }

        [Required] // Validation des formats des données entrantes pour le champ StartDate (data annotations)
        public required DateTime StartDate { get; set; }
        public required DateTime? EndDate { get; set; }

        [Required]
        public required string Category { get; set; }

    }
}
