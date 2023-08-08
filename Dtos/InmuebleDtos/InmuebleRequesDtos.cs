namespace NetKubernetes.Dtos.InmuebleDtos;

public class InmuebleRequestDtos
{
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public decimal Precio { get; set; }
    public string? Imagen { get; set; }
}