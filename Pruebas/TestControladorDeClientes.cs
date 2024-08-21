using ClaseControlador;
using Moq;
using Neo4j.Driver;
using PruebaDeMigrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Pruebas
{
    [TestClass]
    public class ControladorDeClientesTests
    {
        private ControladorDeClientes _controladorDeClientes;

        [TestInitialize]
        public void TestInitialize()
        {

            _controladorDeClientes = new ControladorDeClientes();
        }

        [TestMethod]
        public async Task ObtenerClientesSimuladosAsync_DeberiaRetornarListaDeClientes()
        {
            // Usa el método que devuelve datos simulados
            var resultado = await _controladorDeClientes.ObtenerClientesSimuladosAsync();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Juan Perez", resultado[0].Nombre);
            Assert.AreEqual(123456, resultado[0].Cedula);
        }
    }
}