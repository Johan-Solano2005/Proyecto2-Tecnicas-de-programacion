using ClaseModelo;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseControlador
{
    /// <summary>
    /// Controlador de la clase de Historial
    /// </summary>
    public class ControladorDeHistorial
    {
        private readonly IDriver _driver;

        public ControladorDeHistorial()
        {
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "12345678"));
        }

        public async Task<List<Historial>> ObtenerHistorialAsync(DateTime fecha)
        {
            var historialList = new List<Historial>();

            try
            {
                //comvierte fecha
                var fechaString = fecha.ToString("yyyy-MM-dd");

                var query = @"
                MATCH (h:Historial)
                WHERE date(h.Fecha) = date($fecha)
                RETURN id(h) AS nodeId, h";

                using (var session = _driver.AsyncSession())
                {
                    var parameters = new Dictionary<string, object>
                {
                    { "fecha", fechaString }
                };

                    var cursor = await session.RunAsync(query, parameters);

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

        
        public async Task<List<Historial>> ObtenerHistorialAsync()
        {
            return await ObtenerHistorialAsync(DateTime.Now.Date); 
        }

        public async Task AgregarHistorialAsync(Historial historial)
        {
            try
            {
                var query = "CREATE (h:Historial {Cliente: $cliente, Cantidad: $cantidad, Producto: $producto, Fecha: $fecha}) RETURN id(h) as nodeId";
                var parameters = new Dictionary<string, object>
            {
                { "cliente", historial.Cliente },
                { "cantidad", historial.Cantidad },
                { "producto", historial.Producto },
                { "fecha", historial.Fecha }
            };

                using (var session = _driver.AsyncSession())
                {
                    var cursor = await session.RunAsync(query, parameters);
                    var record = await cursor.SingleAsync();
                    var nodeId = record["nodeId"].As<long>();
                  
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar historial: {ex.Message}");
            }
        }
    }

}

