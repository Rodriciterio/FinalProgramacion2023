using FinalProgramacion2023.Entidades;

namespace FinalProgramacion2023.Windows
{
    public partial class frmTrianguloAE : Form
    {
        public frmTrianguloAE()
        {
            InitializeComponent();
        }

        private Triangulo triangulo;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarComboColores();
            if (triangulo != null)
            {
                txtLadoA.Text = triangulo.LadoA.ToString();
                txtLadoB.Text = triangulo.LadoB.ToString();
                txtLadoC.Text = triangulo.LadoC.ToString();
                cboRelleno.SelectedItem = triangulo.TipoDeRelleno;
                SelectRadioButton(triangulo.TipoDeBorde);
            }
        }

        private void SelectRadioButton(TipoDeBorde tipoDeBorde)
        {
            var key = $"rbt{tipoDeBorde.ToString()}";
            var rb = (RadioButton)gbxBorde.Controls.Find(key, true)[0];
            rb.Checked = true;
        }

        private void CargarComboColores()
        {
            var listaColores = Enum.GetValues(typeof(TipoDeRelleno))
               .Cast<TipoDeRelleno>().ToList();
            cboRelleno.DataSource = listaColores;
            cboRelleno.SelectedIndex = 0;
        }

        public Triangulo GetTriangulo()
        {
            return triangulo;
        }

        public void SetTriangulo(Triangulo? triangulo)
        {
            this.triangulo = triangulo;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (triangulo == null)
                {
                    triangulo = new Triangulo();
                }
                triangulo.LadoA = (int.Parse(txtLadoA.Text));
                triangulo.LadoB = (int.Parse(txtLadoB.Text));
                triangulo.LadoC = (int.Parse(txtLadoC.Text));
                triangulo.TipoDeRelleno = (TipoDeRelleno)cboRelleno.SelectedItem;
                triangulo.TipoDeBorde = ObtenerTipoDeBordes();
                DialogResult = DialogResult.OK;
            }
        }


        List<TipoDeBorde> listaBordes = Enum.GetValues(typeof(TipoDeBorde))
                .Cast<TipoDeBorde>().ToList();
        private TipoDeBorde ObtenerTipoDeBordes()
        {
            TipoDeBorde tipo = 0;
            foreach (var itemBorde in listaBordes)
            {
                var key = $"rbt{itemBorde.ToString()}";
                var rb = (RadioButton)gbxBorde.Controls.Find(key, true)[0];
                if (rb.Checked)
                {
                    tipo = itemBorde;
                    break;
                }
            }
            return tipo;
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (!int.TryParse(txtLadoA.Text, out int ladoA))
            {
                valido = false;
                errorProvider1.SetError(txtLadoA, "Numero mal ingresado");
            }
            else if (ladoA <= 0)
            {
                valido = false;
                errorProvider1.SetError(txtLadoA, "Numero no valido");
            }
            if (!int.TryParse(txtLadoB.Text, out int ladoB))
            {
                valido = false;
                errorProvider1.SetError(txtLadoB, "Numero mal ingresado");
            }
            else if (ladoB <= 0)
            {
                valido = false;
                errorProvider1.SetError(txtLadoB, "Numero no valido");
            }
            if (!int.TryParse(txtLadoC.Text, out int ladoC))
            {
                valido = false;
                errorProvider1.SetError(txtLadoC, "Numero mal ingresado");
            }
            else if (ladoC <= 0)
            {
                valido = false;
                errorProvider1.SetError(txtLadoC, "Numero no valido");
            }
            return valido;
        }
    }
}
