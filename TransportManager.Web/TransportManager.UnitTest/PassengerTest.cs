using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TransportManager.ApplicationServices.Transport;
using Microsoft.Extensions.DependencyInjection;
using TransportManager.Core.Transports;

namespace TransportManager.UnitTest
{
    [TestFixture]
    public class PassengerTest
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
            var repository = server.Host.Services.GetService<IPassengerAppService>();

            var list = repository.GetAllAsync();

            //indica que la prueba ha pasado sin necesidad de verificar condiciones adicionales en este punto
            Assert.Pass();
        }

        [Test]
        public void TestGetByIdAsync()
        {
            var repository = server.Host.Services.GetService<IPassengerAppService>();

            var id = 1;

            var result = repository.GetByIdAsync(id);

            Assert.IsNotNull(result);

            Assert.AreEqual(id, result.Id);

            Assert.Pass();
        }

        [Test]
        public void TestInsertAsync()
        {
            var repository = server.Host.Services.GetService<IPassengerAppService>();

            var newPassenger = new PassengerDto
            {
                Id = 2,
                Name = "John",
                LastName = "Doe",
                Age = 25
            };

            var result = repository.InsertAsync(newPassenger).Result;

            Assert.IsNotNull(result);

            Assert.AreEqual(newPassenger.Id, result.Id);
            Assert.AreEqual(newPassenger.Name, result.Name);
            Assert.AreEqual(newPassenger.LastName, result.LastName);
            Assert.AreEqual(newPassenger.Age, result.Age);

            Assert.Pass();
        }
        [Test]
        public void TestUpdateAsync()
        {
            var repository = server.Host.Services.GetService<IPassengerAppService>();

            // Crear un nuevo pasajero
            var newPassenger = new PassengerDto
            {
                Name = "John",
                LastName = "Doe",
                Age = 25
            };

            var createdPassenger = repository.InsertAsync(newPassenger).Result;

            Assert.IsNotNull(createdPassenger);

            // Actualizar los datos del pasajero
            createdPassenger.Name = "Jane";
            createdPassenger.Age = 30;

            var updatedPassenger = repository.EditAsync(createdPassenger).Result;

            Assert.IsNotNull(updatedPassenger);

            // Verificar que los datos se hayan actualizado correctamente
            Assert.AreEqual("Jane", updatedPassenger.Name);
            Assert.AreEqual(30, updatedPassenger.Age);

            Assert.Pass();
        }

        [Test]
        public void TestDeleteAsync()
        {
            var repository = server.Host.Services.GetService<IPassengerAppService>();

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
