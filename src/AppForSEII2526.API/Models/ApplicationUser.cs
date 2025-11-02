using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    public ApplicationUser()
    {
    }

    public ApplicationUser(string id,string nombreUsuario, string? apellidosUsuario, string direccionDeEnvio)
    {
        Id = id;
        NombreUsuario = nombreUsuario;
        ApellidosUsuario = apellidosUsuario;
        DireccionDeEnvio = direccionDeEnvio;
    }




    //NOMBRE USUARIO
    [Key]
    [StringLength(40, ErrorMessage = "El nombre del usuario no puede ser superior a 40 carecteres")]
    public string NombreUsuario { get; set; }

    //APELLIDOS 
    [StringLength(40, ErrorMessage = "Los apellidos del usuario no pueden ser superiores a 40 carecteres")]
    public string? ApellidosUsuario { get; set; }

    //DIRECCION
    [Required]
    [StringLength(100, ErrorMessage = "La direccion no puede ser superior a 100 carecteres")]
    public string DireccionDeEnvio { get; set; }

    public IList<Compra> Compra { get; set; }
    public IList<Alquiler> Alquiler { get; set; }
    public IList<Reseña> Reseña { get; set; }
}