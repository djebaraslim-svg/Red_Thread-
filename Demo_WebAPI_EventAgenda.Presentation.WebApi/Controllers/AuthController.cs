using Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Services;
using Demo_WebAPI_EventAgenda.Domain.Models;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Dto.Request;
using Demo_WebAPI_EventAgenda.Presentation.WebApi.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMemberService _memberService;
        private readonly TokenTool _tokenTool;

        public AuthController(IMemberService memberService, TokenTool tokenTool)
        {
            _memberService = memberService;
            _tokenTool = tokenTool;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] AuthRegisterRequestDto dto)
        {
            Member member = new Member(
                dto.Pseudonym,
                dto.Email,
                dto.AllowNewsLetter,
                dto.Password                 
                );

            _memberService.Register(member);

            return Ok(new
            {
                Message = "Votre compte a bien été crée!"
                // Génération de token ici
            });

        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthLoginRequestDto dto)
        {
            Member member = _memberService.Login(dto.Email, dto.Password);
            
            string token = _tokenTool.Generate(new TokenTool.Data()
                {
                    MemberId = member.Id,
                    Role = member.Role.ToString()
                // Role = dto.Email == "della@test.be" ? "Admin" : "Péon" // Juste pour la démo, on attribue le rôle d'admin à Della, et les autres sont des péons
            });
                
            return Ok(new
            {
                Message = "Bravo, vous avez mis des crédentials valide",
                Token = token
            });

        }    
    }
}
