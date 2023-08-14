using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext : DbContext
    // Creamos la clase y ponemos : DbContext para que herede de esa clase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        // Aplicamos injeccion de dependencia. Creamos constructor poniendo ctor en el teclado y añadimos : base(options) porque hereda del padre
        }
        public DbSet<Villa> Villas { get; set; }
        // Utilizamos metodo DbSet<> para crear una tabla en la Db cogiendo el modelo Villa y llamandolo Villas en la Db

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la Villa",
                    ImageUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la Villa",
                    ImageUrl = "",
                    Ocupantes = 2,
                    MetrosCuadrados = 30,
                    Tarifa = 650,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                }
            );
        // Utilizamos metodo override OnModelCreating que ya existe en la clase DbContext para cambiar caracteristicas
        }
    }
}
