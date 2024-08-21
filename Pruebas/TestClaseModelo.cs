using ClaseModelo;
using PruebaDeMigrar;

namespace Pruebas
{
    [TestClass]
    public class TestClaseModelo
    {

        /// <summary>
        /// porbar el modelo de Clientes para ver si trae y jala bien los clientes
        /// </summary>
        [TestMethod]
        public void Clientes()
        {
            // Arrange
            var cliente = new Clientes
            {
                Nombre = "Juanito Aliamaña",
                Cedula = 12345678,
                TotalGastado = 1500.75m
            };

            // Act
            var nombre = cliente.Nombre;
            var cedula = cliente.Cedula;
            var totalGastado = cliente.TotalGastado;

            // Assert
            Assert.AreEqual("Juanito Aliamaña", nombre);
            Assert.AreEqual(12345678, cedula);
            Assert.AreEqual(1500.75m, totalGastado);
        }

        // <summary>
        /// porbar el modelo de Historial para ver si trae y jala bien los clientes
        /// </summary>
        [TestMethod]
        public void Historial()
        {
            // Arrange
            var fecha = new DateTime(2024, 8, 22, 10, 12, 50);
            var historial = new Historial
            {
                Id = "1",
                Cliente = "Juanito Alimaña",
                Cantidad = 5,
                Producto = "Pruebas",
                Fecha = fecha
            };

            // Act
            var id = historial.Id;
            var cliente = historial.Cliente;
            var cantidad = historial.Cantidad;
            var producto = historial.Producto;
            var fechaObtenida = historial.Fecha;

            // Assert
            Assert.AreEqual("1", id);
            Assert.AreEqual("Juanito Alimaña", cliente);
            Assert.AreEqual(5, cantidad);
            Assert.AreEqual("Pruebas", producto);
            Assert.AreEqual(fecha, fechaObtenida);
        }

        // <summary>
        /// porbar el modelo de Producto para ver si trae y jala bien los clientes
        /// </summary>

        [TestMethod]
        public void Producto()
        {
            // Arrange
            var producto = new Producto
            {
                Cantidad = 10,
                Nombre = "Martillo",
                Precio = 29.99
            };

            // Act
            var cantidad = producto.Cantidad;
            var nombre = producto.Nombre;
            var precio = producto.Precio;

            // Assert
            Assert.AreEqual(10, cantidad);
            Assert.AreEqual("Martillo", nombre);
            Assert.AreEqual(29.99, precio);
        }
        [TestClass]
        public class TopVendedoresTests
        {
            // <summary>
            /// porbar el modelo de TopVendedores para ver si trae y jala bien los clientes
            /// </summary>
            [TestMethod]
            public void TopVendedores()
            {
                // Arrange
                var topVendedor = new TopVendedores
                {
                    Nombre = "Juan Pérez",
                    CantidadVendida = 150
                };

                // Act
                var nombre = topVendedor.Nombre;
                var cantidadVendida = topVendedor.CantidadVendida;

                // Assert
                Assert.AreEqual("Juan Pérez", nombre);
                Assert.AreEqual(150, cantidadVendida);
            }
        }

    }
}
