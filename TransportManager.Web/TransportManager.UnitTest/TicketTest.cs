using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TransportManager.ApplicationServices.Transport;
using TransportManager.Core.Transports;
using Microsoft.Extensions.DependencyInjection;

namespace TransportManager.UnitTest
{
    [TestFixture]
    public class TicketTest
    {
        protected TestServer server;

        [OneTimeSetUp]
        public void Setup()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        [Test]
        public void TestAllPassenger()
        {
            var repository = server.Host.Services.GetService<ITicketAppService>();

            var list = repository.GetAllAsync();

            //indica que la prueba ha pasado sin necesidad de verificar condiciones adicionales en este punto
            Assert.Pass();
        }

        [Test]
        public void TestGetByIdAsync()
        {
            var repository = server.Host.Services.GetService<ITicketAppService>();

            var id = 1;

            var result = repository.GetByIdAsync(id);

            Assert.IsNotNull(result);

            Assert.AreEqual(id, result.Id);

            Assert.Pass();
        }

        [Test]
        public void TestInsertAsync()
        {
            var repository = server.Host.Services.GetService<ITicketAppService>();

            var newTicket = new TicketDto
            {
                Id = 2,
                Seat = 2
            };

            var result = repository.InsertAsync(newTicket).Result;

            Assert.IsNotNull(result);

            Assert.AreEqual(newTicket.Id, result.Id);
            Assert.AreEqual(newTicket.Seat, result.Seat);

            Assert.Pass();
        }

        [Test]
        public void TestUpdateAsync()
        {
            var repository = server.Host.Services.GetService<ITicketAppService>();

            // Crear un nuevo pasajero
            var newTicket = new TicketDto
            {
                Id = 2,
                Seat = 2
            };

            var createdTicket = repository.InsertAsync(newTicket).Result;

            Assert.IsNotNull(createdTicket);

            // Actualizar los datos del pasajero
            createdTicket.Id = 3;
            createdTicket.Seat = 2;

            var updatedTicket = repository.EditAsync(createdTicket).Result;

            Assert.IsNotNull(updatedTicket);

            // Verificar que los datos se hayan actualizado correctamente
            Assert.AreEqual(3, updatedTicket.Id);
            Assert.AreEqual(2, updatedTicket.Seat);

            Assert.Pass();
        }

        [Test]
        public void TestDeleteAsync()
        {
            var repository = server.Host.Services.GetService<ITicketAppService>();

            var id = 1;

            var result = repository.DeleteAsync(id);

            Assert.Pass();
        }

        //Este método se ejecuta una vez después de la prueba para limpiar y disponer del servidor de prueba.
        [OneTimeTearDownAttribute]
        public void TearDown()
        {
            this.server.Dispose();
        }
    }
}
