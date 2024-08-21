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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PruebaDeMigrar
{
    public partial class Ventas : Form
    {
        private readonly ControladorDeVentas _claseVentas;  
        private readonly List<Producto> _productosSeleccionados;

        private List<TopVendedores> cajeros;


        public Ventas()
        {
            InitializeComponent();
            _claseVentas = new ControladorDeVentas();
            _productosSeleccionados = new List<Producto>();
            Load += Ventas_Load;
            button1.Click += Button1_Click;
            dataGridView1.CellClick += DataGridView1_CellClick;
            comboBox2.Items.Add("Efectivo");
            comboBox2.Items.Add("Tarjeta");
            comboBox2.Items.Add("Transferencia");


        }

        private async void Ventas_Load(object sender, EventArgs e)
        {
            await CargarProductos();
            await CargarCajeros();

        }

        private async Task CargarProductos()
        {
            try
            {
                var productos = await _claseVentas.ObtenerProductos();
                dataGridView1.DataSource = productos;
                dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
                dataGridView1.Columns["Precio"].HeaderText = "Precio";
                dataGridView1.Columns["Cantidad"].HeaderText = "Cantidad";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                var row = dataGridView1.Rows[e.RowIndex];
              
            }
        }
        private async Task CargarCajeros()
        {
            try
            {
                cajeros = await _claseVentas.ObtenerVendedores();
                if (cajeros != null && cajeros.Count > 0)
                {
                    comboBox1.DataSource = cajeros;
                    comboBox1.DisplayMember = "Nombre";
                }
                else
                {
                    MessageBox.Show("No se encontraron cajeros en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cajeros: {ex.Message}");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                var nombre = row.Cells["Nombre"].Value.ToString();
                var cantidadSeleccionada = textBox3.Text;

                if (string.IsNullOrEmpty(cantidadSeleccionada) || !int.TryParse(cantidadSeleccionada, out int cantidad))
                {
                    MessageBox.Show("Por favor, ingrese una cantidad válida.");
                    return;
                }

                if (cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor que cero.");
                    return;
                }

                var producto = new Producto
                {
                    Nombre = nombre,
                    Cantidad = cantidad,
                    Precio = Convert.ToDouble(row.Cells["Precio"].Value)
                };

                var productoExistente = _productosSeleccionados.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
                if (productoExistente != null)
                {
                    productoExistente.Cantidad += cantidad;
                }
                else
                {
                    _productosSeleccionados.Add(producto);
                }

                dataGridView2.DataSource = null;
                dataGridView2.DataSource = _productosSeleccionados;

                dataGridView2.Columns["Nombre"].DisplayIndex = 0;
                dataGridView2.Columns["Cantidad"].DisplayIndex = 1;
                dataGridView2.Columns["Precio"].DisplayIndex = 2;
                dataGridView2.Columns["Nombre"].HeaderText = "Nombre";
                dataGridView2.Columns["Cantidad"].HeaderText = "Cantidad";
                dataGridView2.Columns["Precio"].HeaderText = "Precio";

                // Actualizar el total del carrito
                ActualizarTotalCarrito();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto.");
            }
        }

        private void ActualizarTotalCarrito()
        {
           
            decimal total = _productosSeleccionados.Sum(p => (decimal)p.Precio * p.Cantidad);

           
            label11.Text = $"Total: ¢{total:N2}";
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("El carrito de compras está vacío.");
                return;
            }

            
            string nombreCliente = textBox1.Text;
            if (string.IsNullOrEmpty(nombreCliente))
            {
                MessageBox.Show("Por favor, ingrese el nombre del cliente.");
                return;
            }

         
            if (!int.TryParse(textBox2.Text, out int cedulaCliente))
            {
                MessageBox.Show("Por favor, ingrese una cédula válida.");
                return;
            }

        
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un cajero.");
                return;
            }

            try
            {
             
                bool clienteExiste = await _claseVentas.ClienteExiste(nombreCliente, cedulaCliente);

               
                decimal totalVenta = _productosSeleccionados.Sum(p => (decimal)p.Precio * p.Cantidad);

               
                if (clienteExiste)
                {
                    await _claseVentas.ActualizarTotalGastadoCliente(nombreCliente, totalVenta);
                }
                else
                {
                    var nuevoCliente = new Clientes
                    {
                        Nombre = nombreCliente,
                        Cedula = cedulaCliente,
                        TotalGastado = totalVenta
                    };

                    await _claseVentas.AgregarCliente(nuevoCliente);
                }

                
                var cajeroSeleccionado = comboBox1.SelectedItem as TopVendedores;
                if (cajeroSeleccionado != null)
                {
                    await _claseVentas.ActualizarCantidadVendida(cajeroSeleccionado.Nombre, totalVenta);
                }

                
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.IsNewRow) continue;

                    string nombreProducto = row.Cells["Nombre"].Value.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);

                    await _claseVentas.ActualizarTotalGastadoCliente(nombreProducto, -cantidad);
                }

             
                var controladorDeHistorial = new ControladorDeHistorial();
                foreach (var producto in _productosSeleccionados)
                {
                    var historial = new Historial
                    {
                        Cliente = nombreCliente,
                        Cantidad = producto.Cantidad,
                        Producto = producto.Nombre,
                        Fecha = DateTime.Now
                        
                    };

                    await controladorDeHistorial.AgregarHistorialAsync(historial);
                }

                
                dataGridView2.DataSource = null;
                label11.Text = "Total: ¢0.00";

               
                var metodoPago = comboBox2.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(metodoPago))
                {
                    MessageBox.Show("Por favor, seleccione un método de pago.");
                    return;
                }

             
                var factura = new Factura
                {
                    NombreCliente = nombreCliente,
                    CedulaCliente = cedulaCliente,
                    TotalCompra = totalVenta.ToString("C2"),
                    Cajero = cajeroSeleccionado?.Nombre,
                    MetodoPago = metodoPago,
                    ProductosCarrito = _productosSeleccionados
                };

                factura.Show();

                MessageBox.Show("Pago realizado exitosamente y carrito de compras vacío.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el pago: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var row = dataGridView2.SelectedRows[0];
                var nombreProducto = row.Cells["Nombre"].Value.ToString();

                var producto = _productosSeleccionados.FirstOrDefault(p => p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));
                if (producto != null)
                {
                    _productosSeleccionados.Remove(producto);

                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = _productosSeleccionados;

                    dataGridView2.Columns["Nombre"].DisplayIndex = 0;
                    dataGridView2.Columns["Cantidad"].DisplayIndex = 1;
                    dataGridView2.Columns["Precio"].DisplayIndex = 2;
                    dataGridView2.Columns["Nombre"].HeaderText = "Nombre";
                    dataGridView2.Columns["Cantidad"].HeaderText = "Cantidad";
                    dataGridView2.Columns["Precio"].HeaderText = "Precio";

         
                    ActualizarTotalCarrito();

                    MessageBox.Show("Producto eliminado del carrito.");
                }
                else
                {
                    MessageBox.Show("El producto seleccionado no está en el carrito.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.");
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
              
                var vendedorSeleccionado = comboBox1.SelectedItem as TopVendedores;

                if (vendedorSeleccionado != null)
                {
                    
                }
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {
            Inicio volver = new Inicio();
            volver.Show();
            this.Hide();
        }
    }

}

