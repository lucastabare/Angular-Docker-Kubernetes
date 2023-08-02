using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetKubernetes.Models;

namespace NetKubernetes.Token;

public class JwtGenerador : IjwtGenerador
{
    public string CreateToken(Usuario usuario)
    {
        var claims = new List<Claim>{
        new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
        new Claim("UserId", usuario.Id),
        new Claim("UserEmail", usuario.Email!)
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(30),
            SigningCredentials = credenciales
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }
}
