using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Modelo> Modelo { get; set; }
    public DbSet<Dispositivo> Dispositivo { get; set; }
    public DbSet<Compra> Compra { get; set; }
    public DbSet<ItemCompra> ItemCompra { get; set; }
    public DbSet<ItemAlquiler> AlquilarDispositivo { get; set; }
    public DbSet<Alquiler> Alquiler { get; set; }
    public DbSet<ItemReseña> ItemReseña { get; set; }
    public DbSet<Reseña> Reseña { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }

}
