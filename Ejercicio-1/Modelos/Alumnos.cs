using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio_1.Modelos
{
    public enum TipoDocumento { DNI, DU, Pasaporte }

    public class Alumnos
    {
        public long NroDocumento { get; set; }
        public TipoDocumento TipoDoc { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
