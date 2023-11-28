using FinalProgramacion2023.Datos;
using FinalProgramacion2023.Entidades;

namespace FinalProgramacion2023.Windows
{
    public partial class frmPrincipal : Form
    {
        private RepositorioDeTriangulos repo;
        private List<Triangulo> lista;
        int intValor;
        bool filterOn = false;

        public frmPrincipal()
        {
            InitializeComponent();
            repo = new RepositorioDeTriangulos();
            ActualizarCantidadRegistros();
        }

        private void ActualizarCantidadRegistros()
        {
            if (intValor > 0)
            {
                txtCantidad.Text = repo.GetCantidad(intValor).ToString();
            }
            else
            {
                txtCantidad.Text = repo.GetCantidad().ToString();
            }
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmTrianguloAE frm = new frmTrianguloAE() { Text = "Agregar triangulo" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            Triangulo triangulo = frm.GetTriangulo();
            if (!repo.Existe(triangulo))
            {
                repo.Agregar(triangulo);
                txtCantidad.Text = repo.GetCantidad().ToString();
                DataGridViewRow r = ConstruirFila();
                SetearFila(r, triangulo);
                AgregarFila(r);
                MessageBox.Show("Triangulo Agregado", "Mensaje", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Triangulo Existente", "Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, Triangulo triangulo)
        {
            if (triangulo.EsTriangulo())
            {
                r.Cells[colLadoA.Index].Value = triangulo.LadoA;
                r.Cells[colLadoB.Index].Value = triangulo.LadoB;
                r.Cells[colLadoC.Index].Value = triangulo.LadoC;
                r.Cells[colBorde.Index].Value = triangulo.TipoDeBorde;
                r.Cells[colRelleno.Index].Value = triangulo.TipoDeRelleno;
                r.Cells[colPerimetro.Index].Value = triangulo.GetPerimetro();
                r.Cells[colArea.Index].Value = triangulo.GetArea();
                r.Cells[colValidar.Index].Value = triangulo.ValidarTriangulo();
                r.Cells[colTriangulo.Index].Value = triangulo.ObtenerTipoTriangulo();
            }
            else
            {
                r.Cells[colLadoA.Index].Value = triangulo.LadoA;
                r.Cells[colLadoB.Index].Value = triangulo.LadoB;
                r.Cells[colLadoC.Index].Value = triangulo.LadoC;
                r.Cells[colBorde.Index].Value = triangulo.TipoDeBorde;
                r.Cells[colRelleno.Index].Value = triangulo.TipoDeRelleno;
                r.Cells[colValidar.Index].Value = triangulo.ValidarTriangulo();
            }
            
            r.Tag = triangulo;
        }

        private DataGridViewRow ConstruirFila()
        {
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(dgvDatos);
            return r;
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            DialogResult dr = MessageBox.Show("¿Desea borrar el triangulo?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
            {
                return;
            }
            var filaSeleccionada = dgvDatos.SelectedRows[0];
            Triangulo triangulo = filaSeleccionada.Tag as Triangulo;
            repo.Borrar(triangulo);
            txtCantidad.Text = repo.GetCantidad().ToString();
            QuitarFila(filaSeleccionada);
            MessageBox.Show("Trinagulo borrado", "Mensaje", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void QuitarFila(DataGridViewRow filaSeleccionada)
        {
            dgvDatos.Rows.Remove(filaSeleccionada);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            CargarComboColores();
            if (repo.GetCantidad() > 0)
            {
                lista = repo.GetLista();
                MostrarDatosEnGrilla();
            }
        }

        private void CargarComboColores()
        {
            var listaRellenos = Enum.GetValues(typeof(TipoDeRelleno))
               .Cast<TipoDeRelleno>().ToList();
            foreach (var itemRelleno in listaRellenos)
            {
                tsbCboRelleno.Items.Add(itemRelleno);
            }
            tsbCboRelleno.SelectedIndex = 0;
        }

        private void MostrarDatosEnGrilla()
        {
            dgvDatos.Rows.Clear();
            foreach (var triangulo in lista)
            {
                DataGridViewRow r = ConstruirFila();
                SetearFila(r, triangulo);
                AgregarFila(r);
            }
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var filaSeleccionada = dgvDatos.SelectedRows[0];
            Triangulo triangulo = (Triangulo)filaSeleccionada.Tag;
            Triangulo trianguloCopia = (Triangulo)triangulo.Clone();
            frmTrianguloAE frm = new frmTrianguloAE() { Text = "Editar triangulo" };
            frm.SetTriangulo(triangulo);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            triangulo = frm.GetTriangulo();
            if (!repo.Existe(triangulo))
            {
                repo.Editar(trianguloCopia, triangulo);
                SetearFila(filaSeleccionada, triangulo);
                MessageBox.Show("triangulo editado", "Mensaje", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
            }
            else
            {
                SetearFila(filaSeleccionada, trianguloCopia);
                MessageBox.Show("Triangulo Existente", "Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void tsbCboRelleno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tsbCboRelleno.SelectedIndex == -1)
            {
                return;
            }
            var rellenoFiltro = (TipoDeRelleno)tsbCboRelleno.SelectedItem;
            lista = repo.Filtrar(rellenoFiltro);
            MostrarDatosEnGrilla();
            filterOn = true;
            tsbFiltrar.BackColor = Color.Orange;
        }

        private void porAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!filterOn)
            {
                var stringValor = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el valor del area a filtrar",
                    "Filtrar por mayor o igual",
                     "0", 400, 400);
                if (!int.TryParse(stringValor, out intValor))
                {
                    return;
                }
                if (intValor <= 0)
                {
                    return;
                }
                lista = repo.Filtrar(intValor);
                tsbFiltrar.BackColor = Color.Orange;
                filterOn = true;
                MostrarDatosEnGrilla();
                ActualizarCantidadRegistros();
            }
            else
            {
                MessageBox.Show("Filtro aplicado! \nDebe actualizar la grilla",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ascendenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lista = repo.OrdenarAsc();
            MostrarDatosEnGrilla();
            ActualizarCantidadRegistros();
        }

        private void descendenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lista = repo.OrdenarDesc();
            MostrarDatosEnGrilla();
            ActualizarCantidadRegistros();
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            filterOn = false;
            intValor = 0;
            tsbFiltrar.BackColor = SystemColors.Control;
            lista = repo.GetLista();
            MostrarDatosEnGrilla();
            ActualizarCantidadRegistros();
        }
    }
}
