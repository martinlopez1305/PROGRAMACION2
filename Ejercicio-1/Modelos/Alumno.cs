using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio_1.Modelos
{

    public class Alumno
    {
        public long NroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public override string ToString()
        {
            return $"Alumno: {Nombre} {Apellido} nacido el ";
        }
    }
   
}
