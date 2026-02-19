using Demo_WebAPI_EventAgenda.Domain.Models;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Response
{
    // DTO (Data Transfer Object) pour la réponse d'un événement de l'agenda

    // Ce DTO pour renvoyer un élément "AgendaEvent" qui contient tous ces attributs dans une réponse d'API
    public class AgendaEventResponseDto
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
        public required string? Desc { get; set; }
        public required string? Location { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime? EndDate { get; set; }
        public required string Category { get; set; }

    }

    // Ce DTO pour renvoyer une liste d'éléments "AgendaEvent" dans une réponse d'API
    public class AgendaEventListItemResponseDto
    {
        public required long Id { get; set; }
        public required string Name { get; set; }
    
        public required DateTime StartDate { get; set; }
        public required DateTime? EndDate { get; set; }   

    }


}
