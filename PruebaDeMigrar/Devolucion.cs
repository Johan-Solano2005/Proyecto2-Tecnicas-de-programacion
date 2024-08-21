using ClaseControlador;
using ClaseModelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebaDeMigrar
{
    public partial class Devolucion : Form
    {
        private readonly ControladorDeDevolucion _controladorDeDevolucion;

        public Devolucion()
        {
            InitializeComponent();
            _controladorDeDevolucion = new ControladorDeDevolucion();
            Load += Devolucion_Load;
            button1.Click += Button1_Click;
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private async void Devolucion_Load(object sender, EventArgs e)
        {
            try
            {
                var historiales = await _controladorDeDevolucion.ObtenerHistorialesAsync();
                dataGridView1.DataSource = historiales;
                dataGridView1.Columns["Id"].HeaderText = "ID Interno";
                dataGridView1.Columns["Cliente"].HeaderText = "Cliente";
                dataGridView1.Columns["Cantidad"].HeaderText = "Cantidad";
                dataGridView1.Columns["Producto"].HeaderText = "Producto";
                dataGridView1.Columns["Fecha"].HeaderText = "Fecha";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                var id = row.Cells["Id"].Value.ToString();

                try
                {
                    await _controladorDeDevolucion.BorrarHistorialAsync(id);
                    MessageBox.Show("Historial borrado exitosamente.");

                    // Recargar los historiales
                    var historiales = await _controladorDeDevolucion.ObtenerHistorialesAsync();
                    dataGridView1.DataSource = historiales;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al borrar historial: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un historial para borrar.");
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Esta función se usa para manejar eventos de clic en el DataGridView si es necesario
        }
    }
}

