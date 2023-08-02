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
        return TransformUserToUserDto(usuario!);

    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRquestDto request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!);

        await _sigInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);

        return TransformUserToUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRquestDto request)
    {
        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            Telefono = request.Telefono,
            UserName = request.UserName,
        };

        await _userManager.CreateAsync(usuario!, request.Password!);

        return TransformUserToUserDto(usuario);
    }
}
