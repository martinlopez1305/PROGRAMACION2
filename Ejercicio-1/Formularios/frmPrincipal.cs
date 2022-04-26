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
                    ActualizarAlumno();
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
            txtNombre.Text = alumno.Nombre;
            dtpFechaNacimiento.Value = alumno.FechaNacimiento;
        }
        //Cambia el estado de los botones dependiendo del estadod del formulario
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
                    btnCrear.Enabled = false;
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
        // Cambia el estado de los controles del formulario dependiendo de su estado
        private void CambiarEstadoFormulario(EstadoFormulario estado)
        {
            _estadoFormulario = estado;
            if (_estadoFormulario == EstadoFormulario.Visualizando || _estadoFormulario == EstadoFormulario.Vacio)
            {
                CambiarEstadoControles(false);
                if (_estadoFormulario == EstadoFormulario.Vacio) LimpiarControles();
                if (_estadoFormulario == EstadoFormulario.Visualizando) AsignarDatosControles(_alumnoSelec);
            }
            else
            {
                CambiarEstadoControles(true);
            }
            CambiarEstadoBotones();
        }
        //inserta un alumno en la lista
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
        private void ActualizarAlumno()
        {
            try
            {
                _alumnosRepositorio.ActualizarAlumno(_alumnoSelec);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }          
        }
        //Actualiza el datagridview para reflejar los cambios en el control
        private void ActualizarLista()
        {
            var listanueva = new List<Alumno>();
            listanueva.AddRange(_alumnosRepositorio.ListaAlumnos);
            dgvListaAlumnos.DataSource = listanueva;
            dgvListaAlumnos.Refresh();
        }

        private void dgvListaAlumnos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //dgvListaAlumnos.Rows[e.RowIndex];
            _alumnoSelec = _alumnosRepositorio.ObtenerAlumno((long) dgvListaAlumnos.SelectedCells[0].Value);
            if (_estadoFormulario == EstadoFormulario.Vacio) CambiarEstadoFormulario(EstadoFormulario.Visualizando);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            _alumnosRepositorio.BorrarAlumno(_alumnoSelec);
            _alumnoSelec = new Alumno();
        }
    }
}
