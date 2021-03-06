using Ejercicio_1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio_1.Repositorios
{
    public class AlumnosRepositorio
    {
        // creo una lista de alumnos para almacenarlos temporalmente simulando una base de datoss
        public List<Alumno> ListaAlumnos { get; set; }
        public AlumnosRepositorio()
        {
            ListaAlumnos = new List<Alumno>();
        }
        public Alumno ObtenerAlumno(long nroDocumento)
        {
            var alumno = ListaAlumnos.Where(x => x.NroDocumento == nroDocumento).FirstOrDefault();
            return alumno;
        }
        //Metodo para insertar el alumno a la lista
        public void InsertarAlumno(Alumno alumno)
        {
            //Chequeo los datos
            //chequeo que el alumno ingresado contenga nombre y apellido
            if (String.IsNullOrEmpty(alumno.Nombre) || String.IsNullOrEmpty(alumno.Apellido)) throw new Exception("Debe ingresar el Nombre o el Apellido adecuadamente");
            //compruebo que el alumno ingresado no exista en la lista
            if (ListaAlumnos.Where(x =>  x.NroDocumento == alumno.NroDocumento).Count() > 0) throw new Exception("El alumno ingresado ya existe en la lista");
            //agrego el alumno a la lista
            ListaAlumnos.Add(alumno);
        }
        public void ActualizarAlumno(Alumno alumno)
        {
            var index = ListaAlumnos.FindIndex(x => x.NroDocumento == alumno.NroDocumento);
            if (index < 0) throw new Exception("Alumno no encontrado en la lista");
            ListaAlumnos[index] = alumno;
        }
        public void BorrarAlumno(Alumno alumno)
        {

            ListaAlumnos.Remove(alumno);
        }
    }
}
