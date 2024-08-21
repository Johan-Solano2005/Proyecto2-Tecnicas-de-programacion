using ClaseControlador;
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
   public partial class Gestion_de_Clientes : Form
  {
      private readonly ControladorDeClientes _controladorDeClientes;
      private readonly ControladorDeHistorial _controladorDeHistorial;

      public Gestion_de_Clientes()
      {
          InitializeComponent();
          _controladorDeClientes = new ControladorDeClientes();
          _controladorDeHistorial = new ControladorDeHistorial();
          Load += Gestion_de_Clientes_Load;
      }

      private async void Gestion_de_Clientes_Load(object sender, EventArgs e)
      {
          try
          {
             
              var clientes = await _controladorDeClientes.ObtenerClientesAsync();
              dataGridView1.DataSource = clientes;
              dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
              dataGridView1.Columns["Cedula"].HeaderText = "Cédula";
              dataGridView1.Columns["TotalGastado"].HeaderText = "Total Gastado";

             
              var historial = await _controladorDeHistorial.ObtenerHistorialAsync();
              dataGridView2.DataSource = historial;
              dataGridView2.Columns["Id"].HeaderText = "ID Interno";
              dataGridView2.Columns["Cliente"].HeaderText = "Cliente";
              dataGridView2.Columns["Cantidad"].HeaderText = "Cantidad";
              dataGridView2.Columns["Producto"].HeaderText = "Producto";
              dataGridView2.Columns["Fecha"].HeaderText = "Fecha";
          }
          catch (Exception ex)
          {
              MessageBox.Show($"Error al cargar datos: {ex.Message}");
          }
      }

      private void button1_Click(object sender, EventArgs e)
      {
          Inicio volver = new Inicio();
          volver.Show();
          this.Hide();
      }

      private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
          
      }
  }
}
    

