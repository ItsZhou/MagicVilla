using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    // VillaRepositorio hereda todos los metodos del Repositorio.cs(generico) pero a su vez tiene su propio metodo Actualizar
    {
        private readonly ApplicationDbContext _db;

        public NumeroVillaRepositorio(ApplicationDbContext db) : base(db)
        // Inyectamos ApplicationDbContext a traves del constructor y añadimos : base(db) porque hereda de Repositorio.cs que contiene inyeccion de ApplicationDbContext  
        {
            _db = db;
                // Esto lo inicializa
        }
        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroVillas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
