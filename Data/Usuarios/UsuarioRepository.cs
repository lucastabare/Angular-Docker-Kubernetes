using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDtos;
using NetKubernetes.Token;
using NetKubernetes.Models;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _sigInManager;
    private readonly IjwtGenerador _jwtGenerador;
    private readonly AppDbContext _contexto;
    private readonly IUsuarioSesion _usuarioSeison;

    public UsuarioRepository(
       UserManager<Usuario> userManager,
       SignInManager<Usuario> sigInManager,
       IjwtGenerador jwtGenerador,
       AppDbContext contexto,
       IUsuarioSesion usuarioSeison
    )
    {
        _userManager = userManager!;
        _sigInManager = sigInManager!;
        _jwtGenerador = jwtGenerador;
        _contexto = contexto;
        _usuarioSeison = usuarioSeison;
    }

    private UsuarioResponseDto TransformUserToUserDto(Usuario usuario)
    {
        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            UserName = usuario.UserName,
            Token = _jwtGenerador.CreateToken(usuario)
        };
    }
    public async Task<UsuarioResponseDto> GetUsuario()
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSeison.ObtenerUsuarioSesion());

        if (usuario is null)
        {
            throw new MiddleException(HttpStatusCode.Unauthorized,
            new { mensaje = "El Usuario del token no existe en la base de datos" })
        }

        return TransformUserToUserDto(usuario!);

    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRquestDto request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!);

        if (usuario is null)
        {
            throw new MiddleException(HttpStatusCode.Unauthorized,
            new { mensaje = "El email del usuario no existe en la base de datos" })
        }

        var resultado = await _sigInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);

        if (resultado.Succeeded)
        {
            return TransformUserToUserDto(usuario);
        }

        throw new MiddleException(
            HttpStatusCode.Unauthorized,
        new { mensaje = "Las credenciales son incorrectas" })
    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRquestDto request)
    {
        var existeEmail = await _contexto.Users.Where(c => c.Email == request.Email).AnyAsync();

        if (existeEmail)
        {
            throw new MiddleException(
           HttpStatusCode.BadRequest,
           new { mensaje = "El email del usuario ya existe" })
        }

        var existeUserName = await _contexto.Users.Where(c => c.UserName == request.UserName).AnyAsync();

        if (existeUserName)
        {
            throw new MiddleException(
           HttpStatusCode.BadRequest,
           new { mensaje = "El usuario ya existe" })
        }

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            Telefono = request.Telefono,
            UserName = request.UserName,
        };

        var resultado = await _userManager.CreateAsync(usuario!, request.Password!);

        if (resultado.Succeeded)
        {
            return TransformUserToUserDto(usuario);
        }

        throw new Exception("No se pudo registrar el usuario")

    }
}
