using System.Security.Claims;

namespace NetKubernetes.Token;

public class UsuarioSesion : IUsuarioSesion
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string ObtenerUsuarioSesion()
    {
        var username = _httpContextAccessor.HttpContext!.User?.Claims?
        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return username!;
    }
}