using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Repositories;
using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services;
using Demo_WebAPI_EventAgenda.ApplicationCore.Services;
using Demo_WebAPI_EventAgenda.Infrastructure.Database;
using Demo_WebAPI_EventAgenda.Infrastructure.Database.Repositories;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Configs;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.ExceptionHanders;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (injection de dépendances).
// Ajoutez les services nécessaires pour les contrôleurs avec les méthodes suivantes :
// -AddsSingelton : créer une instance unique pour toute l'application et le garde en mémoire.
// -AddScoped : créer une instance unique par requête HTTP.
// -AddTransient : créer une nouvelle instance à chaque fois qu'on en a besoin. 

//DI Configuration : 
// -Service : relier le interface au service.
builder.Services.AddScoped<IAgendaEventService, AgendaEventService>();
builder.Services.AddScoped<IFaqService, FaqService>();
builder.Services.AddScoped<IMemberService, MemberService>();

// -Tool : relier le interface au tool.
builder.Services.AddSingleton<TokenTool>();

// -Repository : relier le interface au repository. 
builder.Services.AddScoped<IAgendaEventRepository, AgendaEventRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IFaqRepository, FaqRepository>();

// -DB Context : relier le interface au dbcontext.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    //La métode GetConnectionString permet de récupérer la chaîne de connexion nommée "Default" à partir du fichier appsettings.json.
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default")); 
});

// Mapping des contollers
builder.Services.AddControllers();

//Gestion des exeptions (Pattern "Exception Handler")
builder.Services.AddExceptionHandler<AgendaEventExceptionHandler>();
builder.Services.AddProblemDetails(); //Obligatoire pour gérer les autres erreurs, si on ne le met pas, il ne sait plus gérer les exceptions

// Configuration de l'authentification par JWT (Json Web Token)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                 {           
                     byte[] secretKey = Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]!);

                     options.TokenValidationParameters = new TokenValidationParameters()
                     {
                         // Valeur valide pour la configuration du token :
                         ValidIssuer = builder.Configuration["Token:Issuer"],
                         ValidAudience = builder.Configuration["Token:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                         // Règles de validation du token :
                         ValidateIssuer = true, // Valider l'émetteur du token
                         ValidateAudience = true, // Valider l'audience du token
                         ValidateIssuerSigningKey = true, // Valider la clé de signature du token
                         ValidateLifetime = true, // Valider la date d'expiration du token
                     };                 
                 
                 });


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// https://learn.microsoft.com/fr-fr/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-10.0#use-document-transformers
builder.Services.AddOpenApi(options => {

options.AddDocumentTransformer((document, context, cancellationToken) =>
{
document.Info = new()
{
Title = "Agenda Event API",
Version = "v1",
Description = "Démo d'une API RESTFull pour le groupe .Net React de DigitalCity"
};
return Task.CompletedTask;
});
options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

});

//----------------------------------------------------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(); //application de la gestion d'erreur que l'on a configuré ci-dessus

app.UseAuthentication(); //application de l'authentification par JWT que l'on a configuré ci-dessus

app.UseAuthorization();

app.MapControllers();

app.Run();

//----------------------------------------------------------------------------------------------------------------------------------


//On peut aussi créer une classe dédiée pour la configuration du schéma de sécurité (voir configs dans présentation): 

/*/internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            // Add the security scheme at the document level
            var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // "bearer" refers to the header name here
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = securitySchemes;
        }
    }
}*/
