using Ejercicio_1.Modelos;
using Ejercicio_1.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio_1.Formularios
{
    public enum EstadoFormulario { Vacio, Visualizando, Crear, Actualizar}
    public partial class frmPrincipal : Form
    {
        public Alumno _alumnoSelec { get; set; }
        public AlumnosRepositorio _alumnosRepositorio { get; set; }
        public EstadoFormulario _estadoFormulario { get; set; }
        public frmPrincipal()
        {
            InitializeComponent();
            _alumnoSelec = new Alumno();
            _alumnosRepositorio = new AlumnosRepositorio();
            _estadoFormulario = EstadoFormulario.Vacio;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Vacio);
            dgvListaAlumnos.DataSource = _alumnosRepositorio.ListaAlumnos;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Crear);

        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            switch (_estadoFormulario)
            {
                case EstadoFormulario.Crear:
                    InsertarAlumno();
                    break;
                case EstadoFormulario.Actualizar:
                    break;
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Vacio);
        }
        //Activa o desactiva los controles del formulario
        private void CambiarEstadoControles(bool estado)
        {
            txtNroDocumento.Enabled = estado;
            txtApellido.Enabled = estado;
            txtNombre.Enabled = estado;
            dtpFechaNacimiento.Enabled = estado;
        }
        //limpia los controles del formulario
        private void LimpiarControles()
        {
            txtNroDocumento.Text = String.Empty;
            txtApellido.Text = String.Empty;
            txtNombre.Text = String.Empty;
            dtpFechaNacimiento.Value = DateTime.Now;
        }
        //Asigna los datos a los controles
        private void AsignarDatosControles(Alumno alumno)
        {
            txtNroDocumento.Text = alumno.NroDocumento.ToString();
            txtApellido.Text = alumno.Apellido;
            txtNombre.Text = alumno.NroDocumento.ToString();
            dtpFechaNacimiento.Value = alumno.FechaNacimiento;
        }

        private void CambiarEstadoBotones()
        {

            switch (_estadoFormulario)
            {
                case EstadoFormulario.Vacio:
                    btnCrear.Enabled = true;
                    btnActualizar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnLimpiar.Enabled = false;
                    btnCancelar.Enabled = false;
                    break;
                case EstadoFormulario.Visualizando:
                    btnCrear.Enabled = true;
                    btnActualizar.Enabled = true;
                    btnBorrar.Enabled = true;
                    btnGuardar.Enabled = false;
                    btnLimpiar.Enabled = false;
                    btnCancelar.Enabled = false;
                    break;
                case EstadoFormulario.Crear:
                    btnCrear.Enabled = false;
                    btnActualizar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = true;
                    btnLimpiar.Enabled = true;
                    btnCancelar.Enabled = true;
                    break;
                case EstadoFormulario.Actualizar:
                    btnCrear.Enabled = false;
                    btnActualizar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = true;
                    btnLimpiar.Enabled = true;
                    btnCancelar.Enabled = true;
                    break;
                default:
                    break;
            }
        }
        private void CambiarEstadoFormulario(EstadoFormulario estado)
        {
            _estadoFormulario = estado;
            if (_estadoFormulario == EstadoFormulario.Visualizando || _estadoFormulario == EstadoFormulario.Vacio)
            {
                CambiarEstadoControles(false);
                if (_estadoFormulario == EstadoFormulario.Vacio) LimpiarControles();
            }
            else
            {
                CambiarEstadoControles(true);
            }
            CambiarEstadoBotones();
        }

        private void InsertarAlumno()
        {
            try
            {
                long nroDoc;
                var alumno = new Alumno();
                var resultado = long.TryParse(txtNroDocumento.Text, out nroDoc);
                if (!resultado) throw new Exception("Debe ingresar solamente numeros");
                alumno.NroDocumento = nroDoc;
                alumno.Nombre = txtNombre.Text;
                alumno.Apellido = txtApellido.Text;
                alumno.FechaNacimiento = dtpFechaNacimiento.Value;
                _alumnosRepositorio.InsertarAlumno(alumno);
                
                CambiarEstadoFormulario(EstadoFormulario.Vacio);
                ActualizarLista();

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }
        private void ActualizarLista()
        {
            var listanueva = new List<Alumno>();
            listanueva.AddRange(_alumnosRepositorio.ListaAlumnos);
            dgvListaAlumnos.DataSource = listanueva;
            dgvListaAlumnos.Refresh();
        }

        
    }
}
