using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo_WebAPI_EventAgenda.Presentation.WebApi.Token
{
    // Class utilitaire pour générer un JWT (Json Web Token)
    //  - Token qui permettra d'identifier un utilisateur
    public class TokenTool
    {
        // Injection de l'outil pour accéder au fichier de config
        private readonly IConfiguration _config;

        public TokenTool(IConfiguration config)
        {
            _config = config;
        }

        // Cette classe représente les données contenues dans le token
        public class Data
        {
            public required long MemberId { get; set; }
            public required string Role {  get; set; }

        }

        // Méthode pour générer le token
        public string Generate(Data data)
        {
            // Création d'un objet de sécurité de type "claim" : ensemble de données qui seront contenues et visible dans le token
            // -> Datas personnalisées : "Clef" et "La réponse est 42" (exemple de données personnalisées)
            Claim[] claims = [
                new Claim("Clef", "La réponse est 42"),
                new Claim(ClaimTypes.NameIdentifier, data.MemberId.ToString()),
                new Claim(ClaimTypes.Role, data.Role)
            ];

            //Création de la signature du token : permet de garantir l'intégrité du token
            // -> Sécurité de type "SymmetricSecurityKey" : clé de signature symétrique (même clé pour signer et vérifier le token)
            string secret = _config["Token:Key"] ?? throw new Exception("La clé de signature du token n'est pas configurée.");
            byte[] key = Encoding.UTF8.GetBytes(secret);
            //- ou :byte[] key = Encoding.UTF8.GetBytes(_config["Token:Key"]!);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512); 

            //Le token
            // -> Métadonnées en plus des données personnalisées pour bien sécuriser le token :
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["Token:Issuer"],      // Issuer : identité qui émet le token
                audience: _config["Token:Audience"],    // Audience :  contexte d'utilisation du token  
                expires: DateTime.Now.AddMinutes(_config.GetValue<int>("Token:Expire")), // Date d'expiration : durée de validité du token
                claims: claims,                             // Données contenues dans le token  
                signingCredentials: signingCredentials //Signature du token : permet de garantir l'intégrité du token
               );

            // Revoi du token sous forme d'une chaîne de caractères
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
