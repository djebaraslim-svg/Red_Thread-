using Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using static Demo_WebAPI_EventAgenda.Domain.BuisnessExceptions.AgendaEventException;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.ExceptionHanders
{
    public class AgendaEventExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Test de garde : on traite uniquement les exceptions de types AgendaEventException)
            if (exception is not AgendaEventException)           
                return false;

            // Ternaire pour choisir les exceptions
            int statusCode = (exception is AgendaEventNotFoundException) ? StatusCodes.Status404NotFound
                : (exception is AgendaEventCreateException) ? StatusCodes.Status422UnprocessableEntity
                : StatusCodes.Status400BadRequest;

            // Creation de l'erreur à envoyer (= objet erreur que l'on veut envoyer)
            ProblemDetails problem = new ProblemDetails()
            {
                Title = "AgendaEvent error !",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            };

            //Clôture de la requête:
            //- Définition du statut de la réponse
            httpContext.Response.StatusCode = problem.Status.Value;
            //-Envoyer le statut de la réponse
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            // Booléen "vrai" pour indiquer que l'exception a été traité
            return true;
        }
    }
}
