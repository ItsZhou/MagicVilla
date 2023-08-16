using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    // VillaRepositorio hereda todos los metodos del Repositorio.cs(generico) pero a su vez tiene su propio metodo Actualizar
    {
        private readonly ApplicationDbContext _db;

        public VillaRepositorio(ApplicationDbContext db) : base(db)
        // Inyectamos ApplicationDbContext a traves del constructor y añadimos : base(db) porque hereda de Repositorio.cs que contiene inyeccion de ApplicationDbContext  
        {
            _db = db;
                // Esto lo inicializa
        }
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.Villas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
