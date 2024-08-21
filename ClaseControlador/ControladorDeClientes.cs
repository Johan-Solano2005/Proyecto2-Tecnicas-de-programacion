using Neo4j.Driver;
using PruebaDeMigrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseControlador
{
    /// <summary>
    /// Controlador de la clase de clieentes
    /// </summary>
    public class ControladorDeClientes
    {
        private readonly IDriver _driver;

        public ControladorDeClientes()
        {
            
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "12345678"));
        }

        public async Task<List<Clientes>> ObtenerClientesAsync()
        {
            var clientes = new List<Clientes>();

            try
            {
                var query = "MATCH (c:Cliente) RETURN c";
                using (var session = _driver.AsyncSession())
                {
                    var cursor = await session.RunAsync(query);

                    while (await cursor.FetchAsync())
                    {
                        var record = cursor.Current;
                        var clienteNode = record["c"].As<INode>();

                        var cliente = new Clientes
                        {
                            Nombre = clienteNode.Properties.ContainsKey("Nombre") ? clienteNode.Properties["Nombre"].As<string>() : string.Empty,
                            Cedula = clienteNode.Properties.ContainsKey("Cedula") ? clienteNode.Properties["Cedula"].As<int>() : 0,
                            TotalGastado = clienteNode.Properties.ContainsKey("TotalGastado") ? clienteNode.Properties["TotalGastado"].As<decimal>() : 0
                        };

                        clientes.Add(cliente);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener clientes: {ex.Message}");
            }

            return clientes;
        }

            public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
