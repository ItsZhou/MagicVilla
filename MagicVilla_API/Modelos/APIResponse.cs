using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        // Creamos metodo que detalle el codigo de estado, en este caso HttpStatusCode y le damos el nombre statusCode donde se va a almacenar el codigo de estado
        public bool IsExitoso { get; set; } = true;
        // Los metodos bool siempre hay que llamarlos con un nombre que empiece por Is, añadimos = true`para que siempre sea exitoso 
        public List<string> ErrorMessages { get; set; }
        // Lista de todos los errrores que se presente
        public object Resultado { get; set; }
        // Metodo de tipo objeto, nos permite almacenar cualquier tipo de lista independientemente de lo que recibamos
    }
}
