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
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Productos mostrar = new Productos();
            mostrar.Show();
            this.Hide();

        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ventas mostrar = new Ventas();
            mostrar.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Gestion_de_Clientes mostrar = new Gestion_de_Clientes();
            mostrar.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reporte mostrar = new Reporte();
            mostrar.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Devolucion mostrar = new Devolucion();
            mostrar.Show();
            this.Hide();
        }
    }
}
