using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static ProgramacionInstructores.Conexion;

namespace ProgramacionInstructores
{
    public partial class FrmVarEntorno : Form
    {
        private Dictionary<string, Button> botonesDisponibles;

        private Dictionary<Button, Color> coloresOriginales = new Dictionary<Button, Color>();
        private Dictionary<Button, Image> imagenesOriginales = new Dictionary<Button, Image>();

        public FrmVarEntorno()
        {
            InitializeComponent();

            // Inicializamos los botones CRUD
            botonesDisponibles = new Dictionary<string, Button>()
            {
                { "Nuevo", btnNuevo },
                { "Editar", btnEditar },
                { "Eliminar", btnEliminar },
                { "Guardar", btnGuardar },
                { "Consultar", btnConsultar },
                { "Imprimir", btnImprimir },
                { "Salir", btnSalir }
            };
        }

        private void FrmVarEntorno_Load(object sender, EventArgs e)
        {
            Conexion conn = new Conexion();
            conn.CargarEstilosDesdeBD();

            comboBotones.Items.AddRange(botonesDisponibles.Keys.ToArray());
            comboBotones.Items.Add("Todos");
            comboBotones.SelectedIndex = 0;

            // Aplicar estilos desde VarGlobal y guardar originales
            foreach (var kvp in botonesDisponibles)
            {
                var nombre = kvp.Key;
                var boton = kvp.Value;

                // Guardar originales
                coloresOriginales[boton] = boton.BackColor;
                imagenesOriginales[boton] = boton.Image;

                // Aplicar desde VarGlobal
                if (VarGlobal.BotonColores.ContainsKey(nombre))
                    boton.BackColor = VarGlobal.BotonColores[nombre];

                if (VarGlobal.BotonIconos.ContainsKey(nombre) && VarGlobal.BotonIconos[nombre] != null)
                    boton.Image = VarGlobal.BotonIconos[nombre];
            }
        }

        private void btnCambiarColor_Click(object sender, EventArgs e)
        {
            if (comboBotones.SelectedItem == null) return;

            string nombre = comboBotones.SelectedItem.ToString();
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            if (nombre == "Todos")
            {
                foreach (var kvp in botonesDisponibles)
                {
                    kvp.Value.BackColor = dlg.Color;
                    VarGlobal.BotonColores[kvp.Key] = dlg.Color;
                }
            }
            else
            {
                var btn = botonesDisponibles[nombre];
                btn.BackColor = dlg.Color;
                VarGlobal.BotonColores[nombre] = dlg.Color;
            }
        }

        private void btnCambiarIcono_Click(object sender, EventArgs e)
        {
            if (comboBotones.SelectedItem == null) return;

            string nombre = comboBotones.SelectedItem.ToString();
            if (nombre == "Todos") return; // evitar error

            var btn = botonesDisponibles[nombre];

            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Imágenes PNG|*.png|Todas las imágenes|*.jpg;*.jpeg;*.bmp;*.gif;*.ico",
                Title = $"Selecciona icono para {nombre}"
            };

            if (open.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(open.FileName);
                btn.Image = img;
                VarGlobal.BotonIconos[nombre] = img;
            }
        }

        private void btnReestablecer_Click(object sender, EventArgs e)
        {
            if (comboBotones.SelectedItem == null) return;

            string nombre = comboBotones.SelectedItem.ToString();

            if (nombre == "Todos")
            {
                foreach (var kvp in botonesDisponibles)
                {
                    var btn = kvp.Value;

                    if (coloresOriginales.ContainsKey(btn))
                    {
                        btn.BackColor = coloresOriginales[btn];
                        VarGlobal.BotonColores[kvp.Key] = coloresOriginales[btn];
                    }

                    if (imagenesOriginales.ContainsKey(btn))
                    {
                        btn.Image = imagenesOriginales[btn];
                        VarGlobal.BotonIconos[kvp.Key] = imagenesOriginales[btn];
                    }
                }
            }
            else
            {
                var btn = botonesDisponibles[nombre];

                if (coloresOriginales.ContainsKey(btn))
                {
                    btn.BackColor = coloresOriginales[btn];
                    VarGlobal.BotonColores[nombre] = coloresOriginales[btn];
                }

                if (imagenesOriginales.ContainsKey(btn))
                {
                    btn.Image = imagenesOriginales[btn];
                    VarGlobal.BotonIconos[nombre] = imagenesOriginales[btn];
                }
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            Conexion conn = new Conexion();
            conn.GuardarEstilosEnBD();
            MessageBox.Show("Estilos guardados correctamente en la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
