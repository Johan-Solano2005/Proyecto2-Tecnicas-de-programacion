using ClaseModelo;
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
    /// Controlador de la clase Reportes
    /// </summary>
    public class ControladorDeReportes
    {
        private readonly IDriver _driver;

        public ControladorDeReportes()
        {
            _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "12345678"));
        }

        public async Task<List<Historial>> ObtenerHistorialesDelDiaAsync(DateTime fecha)
        {
            return await ObtenerHistorialesPorRangoAsync(fecha, fecha);
        }

        public async Task<List<Historial>> ObtenerHistorialesDelMesAsync(DateTime fecha)
        {
            var primerDiaDelMes = new DateTime(fecha.Year, fecha.Month, 1);
            var ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);
            return await ObtenerHistorialesPorRangoAsync(primerDiaDelMes, ultimoDiaDelMes);
        }

        public async Task<List<Historial>> ObtenerHistorialesDelAnoAsync(DateTime fecha)
        {
            var primerDiaDelAno = new DateTime(fecha.Year, 1, 1);
            var ultimoDiaDelAno = new DateTime(fecha.Year, 12, 31);
            return await ObtenerHistorialesPorRangoAsync(primerDiaDelAno, ultimoDiaDelAno);
        }

        private async Task<List<Historial>> ObtenerHistorialesPorRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var historiales = new List<Historial>();

            var query = @"
    MATCH (h:Historial)
    WHERE date(h.Fecha) >= date($fechaInicio) AND date(h.Fecha) <= date($fechaFin)
    RETURN id(h) AS nodeId, h";

            using (var session = _driver.AsyncSession())
            {
                var parameters = new Dictionary<string, object>
        {
            { "fechaInicio", fechaInicio.ToString("yyyy-MM-dd") },
            { "fechaFin", fechaFin.ToString("yyyy-MM-dd") }
        };

                var cursor = await session.RunAsync(query, parameters);

                while (await cursor.FetchAsync())
                {
                    var record = cursor.Current;
                    var node = record["h"].As<INode>();
                    var nodeId = record["nodeId"].As<long>();

                    var historial = new Historial
                    {
                        Id = nodeId.ToString(),
                        Cliente = node.Properties.ContainsKey("Cliente") ? node.Properties["Cliente"].As<string>() : string.Empty,
                        Cantidad = node.Properties.ContainsKey("Cantidad") ? node.Properties["Cantidad"].As<int>() : 0,
                        Producto = node.Properties.ContainsKey("Producto") ? node.Properties["Producto"].As<string>() : string.Empty,
                        Fecha = node.Properties.ContainsKey("Fecha") ? DateTime.Parse(node.Properties["Fecha"].As<string>()) : DateTime.MinValue
                    };

                    historiales.Add(historial);
                }
            }

            return historiales;
        }
        public async Task<List<Clientes>> ObtenerClientesPorTotalGastadoAsync()
        {
            var clientes = new List<Clientes>();

            var query = @"
            MATCH (c:Cliente)
            RETURN c.Nombre AS Nombre, c.Cedula AS Cedula, sum(c.TotalGastado) AS TotalGastado
            ORDER BY TotalGastado DESC";

            using (var session = _driver.AsyncSession())
            {
                var cursor = await session.RunAsync(query);

                while (await cursor.FetchAsync())
                {
                    var record = cursor.Current;

                    var cliente = new Clientes
                    {
                        Nombre = record["Nombre"].As<string>(),
                        Cedula = record["Cedula"].As<int>(),
                        TotalGastado = record["TotalGastado"].As<decimal>()
                    };

                    clientes.Add(cliente);
                }
            }

            return clientes;
        }
        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            var productos = new List<Producto>();

            var query = @"
        MATCH (p:Producto)
        RETURN p";

            using (var session = _driver.AsyncSession())
            {
                var cursor = await session.RunAsync(query);

                while (await cursor.FetchAsync())
                {
                    var record = cursor.Current;
                    var node = record["p"].As<INode>();

                    var producto = new Producto
                    {
                        Nombre = node.Properties.ContainsKey("Nombre") ? node.Properties["Nombre"].As<string>() : string.Empty,
                        Cantidad = node.Properties.ContainsKey("Cantidad") ? node.Properties["Cantidad"].As<int>() : 0,
                        Precio = node.Properties.ContainsKey("Precio") ? node.Properties["Precio"].As<double>() : 0.0
                    };

                    productos.Add(producto);
                }
            }

            return productos;
        }


    }


}

