using ClaseControlador;
using ClaseModelo;
using Neo4j.Driver;
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
    public partial class Reporte : Form
    {
        private readonly ControladorDeReportes _controladorDeReportes;

        public Reporte()
        {
            InitializeComponent();
            _controladorDeReportes = new ControladorDeReportes();
            Load += Reporte_Load;
        }

        private async void Reporte_Load(object sender, EventArgs e)
        {
            try
            {
                var historialesDelDia = await _controladorDeReportes.ObtenerHistorialesDelDiaAsync(DateTime.Now.Date);
                dataGridView1.DataSource = historialesDelDia;

                var historialesDelMes = await _controladorDeReportes.ObtenerHistorialesDelMesAsync(DateTime.Now);
                dataGridView2.DataSource = historialesDelMes;

                var historialesDelAno = await _controladorDeReportes.ObtenerHistorialesDelAnoAsync(DateTime.Now);
                dataGridView3.DataSource = historialesDelAno;

                var clientesPorTotalGastado = await _controladorDeReportes.ObtenerClientesPorTotalGastadoAsync();
                dataGridView4.DataSource = clientesPorTotalGastado;

                var productos = await _controladorDeReportes.ObtenerProductosAsync();
                dataGridView5.DataSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener reporte: {ex.Message}");
            }
        }


       

        private async Task ExportarAArchivoCSV(DataGridView dataGridView, string nombreArchivo)
        {
            var sb = new StringBuilder();

            // Encabezados
            var headers = dataGridView.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText.Replace("\"", "\"\"") + "\"")));

            // Filas
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value.ToString().Replace("\"", "\"\"") + "\"")));
                }
            }

            // Guardar archivo
            try
            {
                await File.WriteAllTextAsync(nombreArchivo, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo CSV: {ex.Message}");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Inicio volver = new Inicio();
            volver.Show();
            this.Hide();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Implementar si es necesario
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            await ExportarAArchivoCSV(dataGridView1, "HistorialDelDia.csv");
            MessageBox.Show("Reporte del día exportado a HistorialDelDia.csv");
        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            await ExportarAArchivoCSV(dataGridView2, "HistorialDelMes.csv");
            MessageBox.Show("Reporte del mes exportado a HistorialDelMes.csv");
        }

        private async void button4_Click_1(object sender, EventArgs e)
        {
            await ExportarAArchivoCSV(dataGridView3, "HistorialDelAno.csv");
            MessageBox.Show("Reporte del año exportado a HistorialDelAno.csv");
        }
    }
}

