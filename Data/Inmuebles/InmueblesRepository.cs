using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Inmuebles;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly AppDbContext _contexto;
    private readonly IUsuarioSesion _usuarioSesion;
    private readonly UserManager<Usuario> _userManager;
    public InmuebleRepository(
        AppDbContext contexto,
        IUsuarioSesion session,
        UserManager<Usuario> userManager
        )
    {
        _contexto = contexto;
        _usuarioSesion = session;
        _userManager = userManager;
    }
    public async Task CreateInmueble(Inmueble Inmueble)
    {
        var usario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

        if (usario is null)
        {
            throw new MiddleException(
                HttpStatusCode.Unauthorized,
                new { mensaje = "El usuario no es valido" }
            );
        }

        if (Inmueble is null)
        {
            throw new MiddleException(
                HttpStatusCode.BadRequest,
                new { mensaje = "Los datos del inmueble son incorrectos" }
            );
        }

        Inmueble.FechaCreacion = DateTime.Now;
        Inmueble.UsuarioId = Guid.Parse(usario!.Id);

        _contexto.Inmuebles!.Add(Inmueble);
    }

    public void DeleteInmueble(int id)
    {
        var Inmueble = _contexto.Inmuebles!
        .FirstOrDefault(c => c.Id == id);

        _contexto.Inmuebles!.Remove(Inmueble!);
    }

    public IEnumerable<Inmueble> GetAllInmuebles()
    {
        return _contexto.Inmuebles!.ToList();
    }

    public Inmueble GetInmuebleById(int id)
    {
        return _contexto.Inmuebles!.FirstOrDefault(c => c.Id == id)!;
    }

    public bool SaveChanges()
    {
        return (_contexto.SaveChanges() >= 0);
    }
}
