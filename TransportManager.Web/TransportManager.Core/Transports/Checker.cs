using Serilog;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TransportManager.Core.Transports
{
    public class Checker
    {
        private readonly HttpClient _httpClient;

        public Checker(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://ticket.com");
        }

        public async Task<bool> CheckJourneyExistence(int journeyId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"journey/{journeyId}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    Log.Error($"Error checking journey existence: {response.StatusCode} - {response.ReasonPhrase}");
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, $"Error checking journey existence: {ex.Message}");
                return true;
            }
        }

        public async Task<bool> CheckPassengerExistence(int passengerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"passenger/{passengerId}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    Log.Error($"Error checking passenger existence: {response.StatusCode} - {response.ReasonPhrase}");
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, $"Error checking passenger existence: {ex.Message}");
                return true;
            }
        }
    }
}