using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebaDeMigrar
{
    public partial class Factura : Form
    {
        public string NombreCliente { get; set; }
        public int CedulaCliente { get; set; }
        public string TotalCompra { get; set; }
        public string Cajero { get; set; }
        public string MetodoPago { get; set; }
        public List<Producto> ProductosCarrito { get; set; }

        public Factura()
        {
            InitializeComponent();
        }

        private void Factura_Load(object sender, EventArgs e)
        {
            
            label2.Text = NombreCliente;
            label3.Text = CedulaCliente.ToString();
            label4.Text = TotalCompra;
            label5.Text = Cajero;
            label9.Text = MetodoPago;
            var culturaCRC = new CultureInfo("es-CR");
            label4.Text = string.Format(culturaCRC, "¢{0:N2}", TotalCompra);
            // Mostrar los productos en el DataGridView
            dataGridView1.DataSource = ProductosCarrito;
            dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["Cantidad"].HeaderText = "Cantidad";
            dataGridView1.Columns["Precio"].HeaderText = "Precio";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ventas volver = new Ventas();
            volver.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}

