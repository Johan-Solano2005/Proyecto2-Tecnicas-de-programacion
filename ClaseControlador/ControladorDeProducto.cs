using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaDeMigrar
{
    /// <summary>
    /// controlador de la clase Producto
    /// </summary>
    public class ControladorDeProducto
    {
        private readonly IDriver _driver;

        public ControladorDeProducto()
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

        public async Task AgregarProducto(string nombre, int cantidad, double precio)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = "CREATE (p:Producto {Nombre: $nombre, Cantidad: $cantidad, Precio: $precio})";
                var parameters = new Dictionary<string, object>
            {
                { "nombre", nombre },
                { "cantidad", cantidad },
                { "precio", precio }
            };
                await session.RunAsync(query, parameters);
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
            
                throw new ApplicationException("Error al agregar el producto", ex);
            }
        }

        public async Task EditarProducto(string nombre, string nuevoNombre = null, int? cantidad = null, double? precio = null)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = "MATCH (p:Producto {Nombre: $nombre}) SET ";
                var parameters = new Dictionary<string, object> { { "nombre", nombre } };

                if (!string.IsNullOrEmpty(nuevoNombre))
                {
                    query += "p.Nombre = $nuevoNombre, ";
                    parameters.Add("nuevoNombre", nuevoNombre);
                }

                if (cantidad.HasValue)
                {
                    query += "p.Cantidad = $cantidad, ";
                    parameters.Add("cantidad", cantidad.Value);
                }

                if (precio.HasValue)
                {
                    query += "p.Precio = $precio ";
                    parameters.Add("precio", precio.Value);
                }

               
                query = query.TrimEnd(' ', ',');
                await session.RunAsync(query, parameters);
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException("Error al editar el producto", ex);
            }
        }

        public async Task EliminarProducto(string nombre)
        {
            try
            {
                var session = _driver.AsyncSession();
                var query = "MATCH (p:Producto {Nombre: $nombre}) DELETE p";
                var parameters = new Dictionary<string, object> { { "nombre", nombre } };
                await session.RunAsync(query, parameters);
                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Error al eliminar el producto", ex);
            }
        }
    }
}