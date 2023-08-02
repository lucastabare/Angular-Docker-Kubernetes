using NetKubernetes.Dtos.UsuarioDtos;

namespace NetKubernetes.Data.Usuarios;

public interface IUsuarioRepository
{
    Task<UsuarioResponseDto> GetUsuario();

    Task<UsuarioResponseDto> Login(UsuarioLoginRquestDto request);

    Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRquestDto request);
}