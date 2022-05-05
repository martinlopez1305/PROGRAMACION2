using Ejercicio_1.Modelos;
using Ejercicio_1.Repositorios;

namespace Ejercicio_1.Formularios
{
    public enum EstadoFormulario { Vacio, Visualizando, Crear, Actualizar}
    public partial class frmPrincipal : Form
    {
        public Alumno _alumnoSelec { get; set; }
        public AlumnosRepositorio _alumnosRepositorio { get; set; }
        public EstadoFormulario _estadoFormulario { get; set; }
        //Constructor del formulario
        public frmPrincipal()
        {
            InitializeComponent();
            _alumnoSelec = new Alumno();
            _alumnosRepositorio = new AlumnosRepositorio();
            _estadoFormulario = EstadoFormulario.Vacio;
        }
        //Evento de carga del formulario principal
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Vacio);
            dgvListaAlumnos.DataSource = _alumnosRepositorio.ListaAlumnos;
        }
        //Evento del boton crear
        private void btnCrear_Click(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Crear);
        }
        //Evento del boton limpiar.. Basicamente limpia los controles
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }
        //Evento del boton guardar cuando se crea o se actualiza
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string estado = _estadoFormulario == EstadoFormulario.Crear ? "crear" : "actualizar";
            DialogResult res = MessageBox.Show($"¿Esta seguro que desea {estado}?", "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
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
        }
        //Evento del boton mostrar
        private void txtMostrar_Click(object sender, EventArgs e)
        {
            if(_alumnoSelec.NroDocumento > 0) MessageBox.Show(_alumnoSelec.ToString(), "Alumno");
        }
        //Evento del boton cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Vacio);
        }
        //Evento del boton borrrar
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("¿Esta seguro que desea borrar?", "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                _alumnosRepositorio.BorrarAlumno(_alumnoSelec);
                _alumnoSelec = new Alumno();
                CambiarEstadoFormulario(EstadoFormulario.Vacio);
            }
        }
        //Evento del boton Actualizar
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CambiarEstadoFormulario(EstadoFormulario.Actualizar);
        }
        //Evento del datagridview que selecciona el alumno de la lista
        private void dgvListaAlumnos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //dgvListaAlumnos.Rows[e.RowIndex];
            _alumnoSelec = _alumnosRepositorio.ObtenerAlumno((long)dgvListaAlumnos.SelectedCells[0].Value);
            if (_estadoFormulario == EstadoFormulario.Vacio) CambiarEstadoFormulario(EstadoFormulario.Visualizando);
        }
        //Evento txtNroDocumento para impedir que el usuario sea capaz de ingresar caracteres que no sean numeros
        private void txtNroDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        //Activa o desactiva los controles del formulario
        private void CambiarEstadoControles(bool estado)
        {
            txtNroDocumento.Enabled = _estadoFormulario == EstadoFormulario.Actualizar ? false: estado;
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
            ActualizarLista(); //actualiza los controles cada ves que se cambia de estado del formulario
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
                alumno.Nombre = txtNombre.Text.Trim();
                alumno.Apellido = txtApellido.Text.Trim();
                alumno.FechaNacimiento = dtpFechaNacimiento.Value;
                _alumnosRepositorio.InsertarAlumno(alumno);
                
                CambiarEstadoFormulario(EstadoFormulario.Vacio);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }
        //Actualizo el alumno seleccionado
        private void ActualizarAlumno()
        {
            try
            {
                _alumnoSelec.Nombre = txtNombre.Text;
                _alumnoSelec.Apellido = txtApellido.Text;
                _alumnoSelec.FechaNacimiento = dtpFechaNacimiento.Value;
                _alumnosRepositorio.ActualizarAlumno(_alumnoSelec);
                CambiarEstadoFormulario(EstadoFormulario.Visualizando);
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

        
    }
}
