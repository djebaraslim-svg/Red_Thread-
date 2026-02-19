using Demo_WebAPI_EventAgenda.Domain.Models;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Request;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Mappers
{
    // Le but de cette classe est de contenir des méthodes d'extension pour réaliser le mapping
    public static class AgendaEventMapper
    {
        // Mapper pour convertir un élement : le model (Domain) vers le DTO (Présentation)
        public static AgendaEventResponseDto ToResponseDto(this AgendaEvent data) //this va relier la méthode à l'objet
        { 
        return new AgendaEventResponseDto
        {
                Id = data.Id,
                Name = data.Name,
                Desc = data.Desc,
                Location = data.Location,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                Category = data.Category.Name
            };

        }

        public static AgendaEventListItemResponseDto ToListResponseDto(this AgendaEvent data) //this va relier la méthode à l'objet
        {
            return new AgendaEventListItemResponseDto
            {
                Id = data.Id,
                Name = data.Name,             
                StartDate = data.StartDate,
                EndDate = data.EndDate,          
            };
        }
        
        // Mapper pourr convertir le RequestDto (Présentation) vers le modèle (Domain)
        public static AgendaEvent ToDomain(this AgendaEventRequestDto dto)
        {
            return new AgendaEvent(
                dto.Name,
                dto.Location,
                dto.Desc,
                dto.StartDate,
                dto.EndDate,
                new EventCategory(dto.Category)
                );
        }




    }
}
