using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadDatabase
{

    public static async Task InsertarData(AppDbContext context, UserManager<Usuario> userManager)
    {
        if (!userManager.Users.Any())
        {
            var usuario = new Usuario
            {
                Nombre = "Lucas",
                Apellido = "Tabare",
                Email = "lucas.tabare@gmail.com",
                UserName = "lucas.tabare",
                Telefono = "3515150153"
            };

            await userManager.CreateAsync(usuario, "319097351LucasTabare");
        }

        if (!context.Inmuebles!.Any())
        {

            context.Inmuebles!.AddRange(
                new Inmueble
                {
                    Nombre = "Casa de Playa",
                    Direccion = "Av. El Sol 32",
                    Precio = 4500M,
                    FechaCreacion = DateTime.Now
                },
                 new Inmueble
                 {
                     Nombre = "Casa del Lago",
                     Direccion = "Av. La Roca 117",
                     Precio = 5000M,
                     FechaCreacion = DateTime.Now
                 }
            );
        }

        context.SaveChanges();
    }
}