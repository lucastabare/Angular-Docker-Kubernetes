using NetKubernetes.Models;

namespace NetKubernetes.Token;

public interface IjwtGenerador {
    string CreateToken(Usuario usuario);
}