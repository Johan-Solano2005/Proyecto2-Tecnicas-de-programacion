using ClaseModelo;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseControlador
{
    public class ControladorDeDevolucion
    {
        private readonly IDriver _driver;

        public ControladorDeDevolucion()
        {
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "12345678"));
        }

        public async Task<List<Historial>> ObtenerHistorialesAsync()
        {
            var historialList = new List<Historial>();

            try
            {
                var query = @"
            MATCH (h:Historial)
            RETURN id(h) AS nodeId, h";

                using (var session = _driver.AsyncSession())
                {
                    var cursor = await session.RunAsync(query);

                    while (await cursor.FetchAsync())
                    {
                        var record = cursor.Current;
                        var node = record["h"].As<INode>();
                        var nodeId = record["nodeId"].As<long>();

                        var historial = new Historial
                        {
                            Id = nodeId.ToString(), // Usar el ID interno de Neo4j
                            Cliente = node.Properties.ContainsKey("Cliente") ? node.Properties["Cliente"].As<string>() : string.Empty,
                            Cantidad = node.Properties.ContainsKey("Cantidad") ? node.Properties["Cantidad"].As<int>() : 0,
                            Producto = node.Properties.ContainsKey("Producto") ? node.Properties["Producto"].As<string>() : string.Empty,
                            Fecha = node.Properties.ContainsKey("Fecha") ? DateTime.Parse(node.Properties["Fecha"].As<string>()) : DateTime.MinValue
                        };

                        historialList.Add(historial);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener historial: {ex.Message}");
            }

            return historialList;
        }

        public async Task BorrarHistorialAsync(string id)
        {
            try
            {
                // Primero obtener el historial a borrar
                var historial = await ObtenerHistorialPorIdAsync(id);
                if (historial == null) throw new Exception("Historial no encontrado.");

                var query = "MATCH (h:Historial) WHERE id(h) = $id DELETE h";
                var parameters = new Dictionary<string, object> { { "id", long.Parse(id) } };

                using (var session = _driver.AsyncSession())
                {
                    await session.RunAsync(query, parameters);
                }

                // Actualizar la cantidad del producto en la base de datos
                await ActualizarCantidadProductoAsync(historial.Producto, historial.Cantidad);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al borrar historial: {ex.Message}");
            }
        }

        private async Task<Historial> ObtenerHistorialPorIdAsync(string id)
        {
            try
            {
                var query = "MATCH (h:Historial) WHERE id(h) = $id RETURN h";
                var parameters = new Dictionary<string, object> { { "id", long.Parse(id) } };

                using (var session = _driver.AsyncSession())
                {
                    var cursor = await session.RunAsync(query, parameters);

                    if (await cursor.FetchAsync())
                    {
                        var record = cursor.Current;
                        var node = record["h"].As<INode>();

                        return new Historial
                        {
                            Id = id,
                            Cliente = node.Properties.ContainsKey("Cliente") ? node.Properties["Cliente"].As<string>() : string.Empty,
                            Cantidad = node.Properties.ContainsKey("Cantidad") ? node.Properties["Cantidad"].As<int>() : 0,
                            Producto = node.Properties.ContainsKey("Producto") ? node.Properties["Producto"].As<string>() : string.Empty,
                            Fecha = node.Properties.ContainsKey("Fecha") ? DateTime.Parse(node.Properties["Fecha"].As<string>()) : DateTime.MinValue
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener historial por ID: {ex.Message}");
            }

            return null;
        }

        private async Task ActualizarCantidadProductoAsync(string nombreProducto, int cantidadAAgregar)
        {
            try
            {
                var query = @"
            MATCH (p:Producto {Nombre: $nombreProducto})
            SET p.Cantidad = p.Cantidad + $cantidadAAgregar
            RETURN p";

                var parameters = new Dictionary<string, object>
            {
                { "nombreProducto", nombreProducto },
                { "cantidadAAgregar", cantidadAAgregar }
            };

                using (var session = _driver.AsyncSession())
                {
                    await session.RunAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar cantidad del producto: {ex.Message}");
            }
        }
    }
}
    




