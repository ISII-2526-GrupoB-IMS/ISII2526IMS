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
}
