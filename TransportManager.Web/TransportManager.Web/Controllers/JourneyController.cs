using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TransportManager.ApplicationServices.Transport;
using TransportManager.Core.Transports;

namespace TransportManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : ControllerBase
    {
        private readonly IJourneyAppService _journeyAppService;
        public JourneyController(IJourneyAppService journeyAppService)
        {
            _journeyAppService = journeyAppService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JourneyDto>>> GetAll()
        {
            try
            {
                Log.Debug("Starting to retrieve all journey.");

                var journeys = await _journeyAppService.GetAllAsync();

                if (journeys == null || !journeys.Any())
                {
                    Log.Warning("No journeys found.");

                    return NotFound("No journeys found.");
                }

                Log.Information("All journeys retrieved successfully.");

                return Ok(journeys);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all journeys.");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JourneyDto>> GetById(int id)
        {
            try
            {
                var journey = await _journeyAppService.GetByIdAsync(id);

                if (journey == null)
                {
                    Log.Warning("Journey with id {JourneyId} not found.", id);

                    return NotFound();
                }

                Log.Information("Journey with id {JourneyId} retrieved successfully.", id);

                return Ok(journey);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the journey with id {JourneyId}.", id);

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<JourneyDto>> Insert([FromBody] JourneyDto journeys)
        {
            try
            {
                if (journeys == null)
                {
                    Log.Warning("Attempted to insert a null journey.");
                    return BadRequest("Journey data is null.");
                }

                Log.Debug("Starting to insert a new journey.");
                var createdJourney = await _journeyAppService.InsertAsync(journeys);

                if (createdJourney == null)
                {
                    Log.Error("Journey insertion failed for journey: {Journey}", journeys);
                    return StatusCode(500, "An error occurred while creating the journey.");
                }

                Log.Information("Journey with id {JourneyId} inserted successfully.", createdJourney.Id);
                return CreatedAtAction(nameof(GetById), new { id = createdJourney.Id }, createdJourney);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while inserting a new journey.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JourneyDto>> Edit(int id, [FromBody] JourneyDto journeys)
        {
            if (id != journeys.Id)
            {
                Log.Warning("Journey ID in URL ({UrlId}) does not match Journey ID in body ({BodyId}).", id, journeys.Id);
                return BadRequest("Journey ID mismatch.");
            }

            try
            {
                Log.Debug("Starting to edit information of Journey with id {JourneyId}.", id);
                var updatedJourney = await _journeyAppService.EditAsync(journeys);

                if (updatedJourney == null)
                {
                    Log.Warning("Journey with id {JourneyId} not found for update.", id);
                    return NotFound("Journey not found.");
                }

                Log.Information("Journey with id {JourneyId} updated successfully.", id);
                return Ok(updatedJourney);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating the information of journey with id {JourneyId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Log.Debug("Starting to delete Journey with id {JourneyId}.", id);
                var result = await _journeyAppService.DeleteAsync(id);

                if (result == 0)
                {
                    Log.Warning("Journey with id {JourneyId} not found for deletion.", id);
                    return NotFound("Journey not found.");
                }

                Log.Information("Journey with id {JourneyId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the journey with id {journeyId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
