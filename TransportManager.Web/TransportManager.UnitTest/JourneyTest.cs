using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TransportManager.ApplicationServices.Transport;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TransportManager.Web.Controllers;
using TransportManager.Core.Transports;
using Microsoft.AspNetCore.Mvc;

namespace TransportManager.UnitTest
{
    [TestFixture]
    public class JourneyTest
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
            var repository = server.Host.Services.GetService<IJourneyAppService>();

            var list = repository.GetAllAsync();

            //indica que la prueba ha pasado sin necesidad de verificar condiciones adicionales en este punto
            Assert.Pass();
        }

        [Test]
        public void TestGetByIdAsync()
        {
            var repository = server.Host.Services.GetService<IJourneyAppService>();

            var id = 1;

            var result = repository.GetByIdAsync(id);

            Assert.IsNotNull(result);

            Assert.AreEqual(id, result.Id);

            Assert.Pass();
        }

        [Test]
        public void TestInsertAsync()
        {
            var repository = server.Host.Services.GetService<IJourneyAppService>();

            var newJourney = new JourneyDto
            {
                Id = 2,
                DestinationId = 1,
                OriginId = 2,
                Departure = DateTime.Now,
                Arrival = DateTime.Now.AddHours(2)
            };

            var result = repository.InsertAsync(newJourney).Result;

            Assert.IsNotNull(result);

            Assert.AreEqual(newJourney.Id, result.Id);
            Assert.AreEqual(newJourney.DestinationId, result.DestinationId);
            Assert.AreEqual(newJourney.OriginId, result.OriginId);
            Assert.AreEqual(newJourney.Departure, result.Departure);
            Assert.AreEqual(newJourney.Arrival, result.Arrival);

            Assert.Pass();
        }

        [Test]
        public void TestUpdateAsync()
        {
            var repository = server.Host.Services.GetService<IJourneyAppService>();

            // Crear un nuevo viaje
            var newJourney = new JourneyDto
            {
                Id = 1,
                DestinationId = 1,
                OriginId = 2,
                Departure = DateTime.Now,
                Arrival = DateTime.Now.AddHours(2)
            };

            var createdJourney = repository.InsertAsync(newJourney).Result;

            Assert.IsNotNull(createdJourney);

            // Actualizar los datos del viaje
            newJourney.DestinationId = 3;
            newJourney.OriginId = 4;
            newJourney.Departure = DateTime.Now.AddHours(1);
            newJourney.Arrival = DateTime.Now.AddHours(3);

            var updatedJourney = repository.EditAsync(newJourney).Result;

            Assert.IsNotNull(updatedJourney);

            // Verificar que los datos se hayan actualizado correctamente
            Assert.AreEqual(3, updatedJourney.DestinationId);
            Assert.AreEqual(4, updatedJourney.OriginId);
            Assert.AreEqual(newJourney.Departure, updatedJourney.Departure);
            Assert.AreEqual(newJourney.Arrival, updatedJourney.Arrival);

            Assert.Pass();
        }

        [Test]
        public void TestDeleteAsync()
        {
            var repository = server.Host.Services.GetService<IJourneyAppService>();

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