using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TransportManager.ApplicationServices.Transport;
using TransportManager.Core.Transports;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;

namespace TransportManager.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketAppService _ticketAppService;
        private readonly HttpClient _httpClient;

        public TicketController(ITicketAppService ticketAppService, HttpClient httpClient)
        {
            _ticketAppService = ticketAppService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://ticket.com");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll()
        {
            try
            {
                Log.Debug("Starting to retrieve all tickets.");

                var tickets = await _ticketAppService.GetAllAsync();

                if (tickets == null || !tickets.Any())
                {
                    Log.Warning("No tickets found.");
                    return NotFound("No tickets found.");
                }

                Log.Information("All tickets retrieved successfully.");
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all tickets.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetById(int id)
        {
            try
            {
                var tickets = await _ticketAppService.GetByIdAsync(id);

                if (tickets == null)
                {
                    Log.Warning("Ticket with id {TicketId} not found.", id);
                    return NotFound();
                }

                Log.Information("Ticket with id {TicketId} retrieved successfully.", id);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving the ticket with id {TicketId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> Insert([FromBody] TicketDto ticket)
        {
            try
            {
                if (ticket == null)
                {
                    Log.Warning("Attempted to insert a null ticket.");
                    return BadRequest("ticket data is null.");
                }

                Log.Debug("Starting to insert a new ticket.");
                var checker = new Checker(_httpClient);

                if (!await checker.CheckJourneyExistence(ticket.Journeys_Id))
                {
                    Log.Warning("Journey with id {journeyId} does not exist.", ticket.Journeys_Id);
                    return BadRequest("Journey does not exist.");
                }

                if (!await checker.CheckPassengerExistence(ticket.Passengers_Id))
                {
                    Log.Warning("Passenger with id {passengerId} does not exist.", ticket.Passengers_Id);
                    return BadRequest("Passenger does not exist.");
                }

                var createdticket = await _ticketAppService.InsertAsync(ticket);

                if (createdticket == null)
                {
                    Log.Error("ticket insertion failed for ticket: {ticket}", ticket);
                    return StatusCode(500, "An error occurred while creating the ticket.");
                }

                Log.Information("ticket with id {ticketId} inserted successfully.", createdticket.Id);
                return CreatedAtAction(nameof(GetById), new { id = createdticket.Id }, createdticket);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while inserting a new ticket.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDto>> Edit(int id, [FromBody] TicketDto tickets)
        {
            if (id != tickets.Id)
            {
                Log.Warning("Ticket ID in URL ({UrlId}) does not match ticket ID in body ({BodyId}).", id, tickets.Id);
                return BadRequest("Ticket ID mismatch.");
            }

            try
            {
                Log.Debug("Starting to edit ticket with id {TicketId}.", id);
                var updatedTicket = await _ticketAppService.EditAsync(tickets);

                if (updatedTicket == null)
                {
                    Log.Warning("Ticket with id {TicketId} not found for update.", id);
                    return NotFound("Ticket not found.");
                }

                Log.Information("Ticket with id {TicketId} updated successfully.", id);
                return Ok(updatedTicket);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating the ticket with id {TicketId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Log.Debug("Starting to delete ticket with id {TicketId}.", id);
                var result = await _ticketAppService.DeleteAsync(id);

                if (result == 0)
                {
                    Log.Warning("Ticket with id {TicketId} not found for deletion.", id);
                    return NotFound("Ticket not found.");
                }

                Log.Information("Ticket with id {TicketId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the ticket with id {TicketId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
