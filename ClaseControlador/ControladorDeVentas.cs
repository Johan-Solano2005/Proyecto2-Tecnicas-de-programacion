using ClaseModelo;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDeMigrar
{
    /// <summary>
    /// Controlador de clase ventas
    /// </summary>
    public class ControladorDeVentas
    {
        private readonly IDriver _driver;

        public ControladorDeVentas()
        {
            
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "12345678"));
        }

       
        public async Task<List<Producto>> ObtenerProductos()
        {
            var productos = new List<Producto>();

            try
            {
             
                var session = _driver.AsyncSession();
                var result = await session.RunAsync("MATCH (p:Producto) RETURN p");

             
                await result.ForEachAsync(record =>
                {
                    var node = record["p"].As<INode>();
                    productos.Add(new Producto
                    {
                        Nombre = node.Properties["Nombre"].As<string>(),
                        Cantidad = node.Properties["Cantidad"].As<int>(),
                        Precio = node.Properties["Precio"].As<double>()
                    });
                });

               
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los productos", ex);
            }

            return productos;
        }

        
        public async Task<bool> ClienteExiste(string nombreCliente, int cedulaCliente)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = @"
            MATCH (c:Cliente {Nombre: $nombre, Cedula: $cedula})
            RETURN COUNT(c) > 0 AS exists
        ";
                var parameters = new
                {
                    nombre = nombreCliente,
                    cedula = cedulaCliente
                };

                var result = await session.RunAsync(query, parameters);
                var record = await result.SingleAsync();
                bool exists = record["exists"].As<bool>();

                await session.CloseAsync();
                return exists;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al verificar la existencia del cliente", ex);
            }
        }

   
        public async Task AgregarCliente(Clientes cliente)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = "CREATE (c:Cliente {Nombre: $nombre, Cedula: $cedula, TotalGastado: $totalGastado})";
                var parameters = new { nombre = cliente.Nombre, cedula = cliente.Cedula, totalGastado = cliente.TotalGastado };
                await session.RunAsync(query, parameters);
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al agregar el cliente", ex);
            }
        }

    
        public async Task ActualizarTotalGastadoCliente(string nombreCliente, decimal montoGastado)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = @"
                    MATCH (c:Cliente {Nombre: $nombre})
                    SET c.TotalGastado = coalesce(c.TotalGastado, 0) + $montoGastado
                ";
                var parameters = new
                {
                    nombre = nombreCliente,
                    montoGastado = montoGastado
                };
                await session.RunAsync(query, parameters);
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al actualizar el total gastado del cliente", ex);
            }
        }

        
        public decimal CalcularTotalCarrito(List<Producto> productosSeleccionados)
        {
            return productosSeleccionados.Sum(p => (decimal)p.Precio * p.Cantidad);
        }

        
        public async Task<List<TopVendedores>> ObtenerVendedores()
        {
            var vendedores = new List<TopVendedores>();

            try
            {
                var session = _driver.AsyncSession();
                var result = await session.RunAsync("MATCH (v:TopVendedores) RETURN v");

                await result.ForEachAsync(record =>
                {
                    var node = record["v"].As<INode>();
                    vendedores.Add(new TopVendedores
                    {
                        Nombre = node.Properties["Nombre"].As<string>(),
                        CantidadVendida = node.Properties["CantidadVendida"].As<int>()
                    });
                });

                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener los vendedores", ex);
            }

            return vendedores;
        }

 
        public async Task ActualizarCantidadVendida(string nombreCajero, decimal cantidadVendida)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = @"
                    MATCH (v:TopVendedores {Nombre: $nombre})
                    SET v.CantidadVendida = v.CantidadVendida + $cantidadVendida
                ";
                var parameters = new
                {
                    nombre = nombreCajero,
                    cantidadVendida = cantidadVendida
                };
                await session.RunAsync(query, parameters);
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al actualizar la cantidad vendida del cajero", ex);
            }
        }

        
        public async Task<List<Clientes>> ObtenerClientesOrdenadosPorGasto()
        {
            var clientes = new List<Clientes>();

            try
            {
                var session = _driver.AsyncSession();
                var result = await session.RunAsync("MATCH (c:Cliente) RETURN c ORDER BY c.TotalGastado DESC");

                await result.ForEachAsync(record =>
                {
                    var node = record["c"].As<INode>();
                    clientes.Add(new Clientes
                    {
                        Nombre = node.Properties["Nombre"].As<string>(),
                        Cedula = node.Properties["Cedula"].As<int>(),
                        TotalGastado = node.Properties["TotalGastado"].As<decimal>()
                    });
                });

                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener clientes ordenados por gasto", ex);
            }

            return clientes;
        }
       


    }
}
