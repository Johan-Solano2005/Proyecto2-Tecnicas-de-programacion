
namespace PruebaDeMigrar
{
    public partial class Productos : Form
    {
        private readonly ControladorDeProducto _neo4jService;

        public Productos()
        {
            InitializeComponent();
            _neo4jService = new ControladorDeProducto();
            this.Load += Form1_Load;
            dataGridView1.CellClick += dataGridView1_CellClick;
            comboBox1.Items.AddRange(new string[] { "Nombre", "Precio", "Cantidad" });
            comboBox1.SelectedIndex = 0;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await CargarProductos();
        }

        private async Task CargarProductos()
        {
            try
            {
                var productos = await _neo4jService.ObtenerProductos();
                if (productos.Count == 0)
                {
                    MessageBox.Show("No se encontraron productos.");
                }
                else
                {
                    dataGridView1.DataSource = productos;

                   
                    dataGridView1.Columns["Nombre"].DisplayIndex = 0;
                    dataGridView1.Columns["Cantidad"].DisplayIndex = 1;
                    dataGridView1.Columns["Precio"].DisplayIndex = 2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var nombre = textBox1.Text;
                if (!double.TryParse(textBox2.Text, out double precio) || !int.TryParse(textBox3.Text, out int cantidad))
                {
                    MessageBox.Show("Por favor, ingrese valores válidos.");
                    return;
                }

               
                var productosExistentes = await _neo4jService.ObtenerProductos();
                if (productosExistentes.Any(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Ya existe un producto con este nombre.");
                    return;
                }

                await _neo4jService.AgregarProducto(nombre, cantidad, precio);

               
                await CargarProductos();

                MessageBox.Show("Producto agregado exitosamente.");

              
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Error en el formato de entrada: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var row = dataGridView1.SelectedRows[0];
                    var nombre = row.Cells["Nombre"].Value.ToString();

                    var campoSeleccionado = comboBox1.SelectedItem?.ToString();
                    var nuevoValor = textBox4.Text;

                    if (string.IsNullOrEmpty(campoSeleccionado) || string.IsNullOrEmpty(nuevoValor))
                    {
                        MessageBox.Show("Por favor, seleccione un campo y un nuevo valor.");
                        return;
                    }

                    switch (campoSeleccionado)
                    {
                        case "Nombre":
                           
                            await _neo4jService.EditarProducto(nombre, nuevoNombre: nuevoValor);
                            break;
                        case "Cantidad":
                            if (int.TryParse(nuevoValor, out int cantidad))
                            {
                                await _neo4jService.EditarProducto(nombre, cantidad: cantidad);
                            }
                            else
                            {
                                MessageBox.Show("Por favor, ingrese un valor válido para la cantidad.");
                                return;
                            }
                            break;
                        case "Precio":
                            if (double.TryParse(nuevoValor, out double precio))
                            {
                                await _neo4jService.EditarProducto(nombre, precio: precio);
                            }
                            else
                            {
                                MessageBox.Show("Por favor, ingrese un valor válido para el precio.");
                                return;
                            }
                            break;
                        default:
                            MessageBox.Show("Campo seleccionado no válido.");
                            return;
                    }

                    await CargarProductos(); 

                    MessageBox.Show("Producto actualizado exitosamente.");

                    textBox4.Clear();
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un producto para actualizar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                var row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["Nombre"].Value.ToString();
                textBox3.Text = row.Cells["Cantidad"].Value.ToString();
                textBox2.Text = row.Cells["Precio"].Value.ToString();
            }
        }




        private async void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener el nombre del producto seleccionado
                var row = dataGridView1.SelectedRows[0];
                var productoNombre = row.Cells["Nombre"].Value.ToString();

                // Confirmar eliminación
                var confirmResult = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Eliminar el producto usando Neo4jService
                        await _neo4jService.EliminarProducto(productoNombre);

                        // Recargar los productos en el DataGridView
                        await CargarProductos();

                        MessageBox.Show("Producto eliminado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para eliminar.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Inicio volver = new Inicio();
            volver.Show();
            this.Hide();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verifica si la columna es la de cantidad
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                if (e.Value != null && int.TryParse(e.Value.ToString(), out int cantidad))
                {
                    // Cambia el color de fondo y el color del texto según la cantidad
                    if (cantidad < 5)
                    {
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Red; // Opcional: Cambia el color del texto para mejor contraste
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.White; // Restaura el color de fondo predeterminado
                        e.CellStyle.ForeColor = Color.Black; // Restaura el color del texto predeterminado
                    }
                }
            }
        }
    }
}