using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services;
using Demo_WebAPI_EventAgenda.Domain.Models;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Mappers;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Request;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaEventController : ControllerBase
    {
        // Injection de la dépendance vers le service de gestion des événements de l'agenda
        private readonly IAgendaEventService _agendaEventService;

        // Injection de dépendance via le constructeur
        public AgendaEventController(IAgendaEventService agendaEventService)
        {
            _agendaEventService = agendaEventService;
        }

        // Endpoint pour récupérer un événement de l'agenda de la DB par son ID
        [HttpGet("{id}")]
        [ProducesResponseType<AgendaEventResponseDto>(200)]
        public IActionResult GetById([FromRoute]long id)
        {
            // Récupération des données via le service dédié "AgendaEventService" (couche ApplicationCore)
            AgendaEvent result = _agendaEventService.GetById(id);

            // Un DTO est une classe spécifique utilisée pour le transfert de données entre les couches de l'application.
            // UN DTO permet de ne pas exposer directement les entités du domaine aux couches externes, comme les API ou les interfaces utilisateur.
            // Un DTO cache les détails internes de l'entité du domaine et expose uniquement les données nécessaires pour une opération spécifique.

            // Transfère des données dans un objet "ResponseDTO" (non implémenté ici) pour l'envoyer en réponse de l'API (vers le client)
            AgendaEventResponseDto dto = new AgendaEventResponseDto()
            {
                Id = result.Id,
                Name = result.Name,
                Desc = result.Desc,
                Location = result.Location,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                Category = result.Category.Name
            };

            // !!!!!! Utilisation du mapper !!!!!!
            // AgendaEventResponseDto dto = result.ToResponseDto();

            // Retourne la réponse DTO au client sous forme de résultat HTTP 200 OK
            return Ok(dto);
        }

        // Endpoint pour ajouter un nouvel événement dans la DB
        [HttpPost]
        [ProducesResponseType<AgendaEventResponseDto>(201)]
        [Authorize]
        public IActionResult AddElement (AgendaEventRequestDto data )
        {
            AgendaEvent agendaEvent = new AgendaEvent(
                data.Name,
                data.Location,
                data.Desc,
                data.StartDate,           
                data.EndDate,
                new EventCategory(data.Category)
            );

           // !!!!!! Utilisation du mapper !!!!!!
           // AgendaEvent agendaEvent = data.ToDomain();

            // Utilisation du service (ApplicationCore) pour ajouter les données
           AgendaEvent result =  _agendaEventService.Create(agendaEvent);

            // Transfère des données dans un objet "RequestDTO" (non implémenté ici) pour l'envoyer en réponse de l'API (vers le client)
            AgendaEventResponseDto dto = new AgendaEventResponseDto()
            {
                Id = result.Id,
                Name = result.Name,
                Desc = result.Desc,
                Location = result.Location,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                Category = result.Category.Name
            };

            // !!! Utilisation du mapper !!!
            // AgendaEventResponseDto dto = result.ToResponseDto();
            
            // !!! Utilisation du mapper !!! TOUT SUR UNE SEULE LIGNE
            //AgendaEventResponseDto dto2 = _agendaEventService.Create(data.ToDomain()).ToResponseDto;

            // Crée une réponse HTTP 201 Created avec un en-tête Location pointant vers le nouvel élément créé
            return CreatedAtAction( 
                nameof(GetById), // Nom de l'action pour récupérer l'élément créé, endpoint GetById pour récupérer l'élément par son ID
                new { id = result.Id },  // Données nécessaires pour construire l'URL de l'élément créé (ID), au endpoint GetById
                dto // Données de l'objet créé à inclure dans le corps de la réponse
                );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        // [ProducesResponseType(204)] : si on utilise le return NoContent() (pas de renvoi de données)
        [Authorize(Roles = "Admin, Manager")] // Seuls les utilisateurs avec le rôle "Admin" peuvent accéder à cet endpoint de suppression
        public IActionResult Delete (int id)
        {
            AgendaEvent result = _agendaEventService.GetById(id);

            AgendaEventResponseDto dto = new AgendaEventResponseDto()
            {
                Id = result.Id,
                Name = result.Name,
                Desc = result.Desc,
                Location = result.Location,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                Category = result.Category.Name
            };

           _agendaEventService.Delete(id);
            return Ok(dto);
        }

        [HttpGet]
        [ProducesResponseType<IEnumerable<AgendaEventListItemResponseDto>>(200)]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int nbElement = 10) //La pagination limite le GetAll
        {
            IEnumerable <AgendaEvent> result = _agendaEventService.GetMany(page, nbElement);

           IEnumerable <AgendaEventListItemResponseDto> dto = result.Select(r => new AgendaEventListItemResponseDto
           {
                Id = r.Id,
                Name = r.Name,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
            });

            //!!! Utilisation du mapper !!!
            //IEnumerable<AgendaEventListItemResponseDto> dto = result.Select(r => r.ToListResponseDto());

            return Ok(dto);
        }

        [HttpGet("date/{starDate}")]
        [ProducesResponseType<IEnumerable<AgendaEventListItemResponseDto>>(200)]
        public IActionResult GetByDate([FromRoute] DateTime startDate)
        {
            IEnumerable <AgendaEvent> result = _agendaEventService.GetAllByDate(startDate);

            IEnumerable<AgendaEventListItemResponseDto> dto = result.Select(r=> new AgendaEventListItemResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                StartDate = r.StartDate,
                EndDate = r.EndDate,

            });

            //!!! Utilisation du mapper deuxième façon !!!
            //IEnumerable<AgendaEventListItemResponseDto> dto = result.Select(AgendaEventMapper.ToListResponseDto);
            
            return Ok(dto);

        }

        [HttpGet("date/{starDate}/to/{endDate}")]
        [ProducesResponseType<IEnumerable<AgendaEventListItemResponseDto>>(200)]
        public IActionResult GetByDate([FromRoute] DateTime startDate, [FromRoute] DateTime endDate)
        {
            IEnumerable<AgendaEvent> result = _agendaEventService.GetAllByDateRange(startDate, endDate);

            IEnumerable<AgendaEventListItemResponseDto> dto = result.Select(r => new AgendaEventListItemResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                StartDate = r.StartDate,
                EndDate = r.EndDate,

            });

            //!!! Utilisation du mapper deuxième façon !!!
            //IEnumerable<AgendaEventListItemResponseDto> dto = result.Select(AgendaEventMapper.ToListResponseDto);

            return Ok(dto);
        }
    }
}
