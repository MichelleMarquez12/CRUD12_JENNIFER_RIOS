using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TransportManager.ApplicationServices.Transport;
using TransportManager.Core.Transports;

namespace TransportManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerAppService _passengerAppService;
        public PassengerController(IPassengerAppService passengerAppService)
        {
            _passengerAppService = passengerAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerDto>>> GetAll()
        {
            try
            {
                Log.Debug("Starting to retrieve all passengers.");

                var passengers = await _passengerAppService.GetAllAsync();

                if (passengers == null || !passengers.Any())
                {
                    Log.Warning("No passengers found.");

                    return NotFound("No passengers found.");
                }

                Log.Information("All passengers retrieved successfully.");

                return Ok(passengers);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all passengers.");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerDto>> GetById(int id)
        {
            try
            {
                var passenger = await _passengerAppService.GetByIdAsync(id);

                if (passenger == null)
                {
                    Log.Warning("Passenger with id {PassengerId} not found.", id);

                    return NotFound();
                }

                Log.Information("Passenger with id {PassengerId} retrieved successfully.", id);

                return Ok(passenger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the passenger with id {PassengerId}.", id);

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PassengerDto>> Insert([FromBody] PassengerDto passenger)
        {
            try
            {
                if (passenger == null)
                {
                    Log.Warning("Attempted to insert a null passenger.");
                    return BadRequest("Passenger data is null.");
                }

                Log.Debug("Starting to insert a new ticket.");
                var createdPassenger = await _passengerAppService.InsertAsync(passenger);

                if (createdPassenger == null)
                {
                    Log.Error("Passenger insertion failed for passenger: {Passenger}", passenger);
                    return StatusCode(500, "An error occurred while creating the passenger.");
                }

                Log.Information("Passenger with id {PassengerId} inserted successfully.", createdPassenger.Id);
                return CreatedAtAction(nameof(GetById), new { id = createdPassenger.Id }, createdPassenger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while inserting a new passenger.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PassengerDto>> Edit(int id, [FromBody] PassengerDto passenger)
        {
            if (id != passenger.Id)
            {
                Log.Warning("Passenger ID in URL ({UrlId}) does not match passenger ID in body ({BodyId}).", id, passenger.Id);
                return BadRequest("Passenger ID mismatch.");
            }

            try
            {
                Log.Debug("Starting to edit information of Passenger with id {PassengerId}.", id);
                var updatedPassenger = await _passengerAppService.EditAsync(passenger);

                if (updatedPassenger == null)
                {
                    Log.Warning("Passenger with id {PassengerId} not found for update.", id);
                    return NotFound("Passenger not found.");
                }

                Log.Information("Passenger with id {PassengerId} updated successfully.", id);
                return Ok(updatedPassenger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating the information of passenger with id {PasssengerId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Log.Debug("Starting to delete Passenger with id {PassengerId}.", id);
                var result = await _passengerAppService.DeleteAsync(id);

                if (result == 0)
                {
                    Log.Warning("Passenger with id {PassengerId} not found for deletion.", id);
                    return NotFound("Passenger not found.");
                }

                Log.Information("Passenger with id {PassengerId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the passsenger with id {PassengerId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
