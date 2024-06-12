using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotRenewalApp.Models;
using System.Net;

namespace MotRenewalApp.Services
{
    public class MotService : IMotService
    {
        private readonly HttpClient _httpClient;
        private readonly MotApiSettings _settings;

        public MotService(HttpClient httpClient, IOptions<MotApiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<Vehicle> FetchMotData(string registrationNumber)
        {
            try
            {
                var requestUrl = $"{_settings.Url}{registrationNumber}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("x-api-key", _settings.ApiKey);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var vehicleDtos = await response.Content.ReadFromJsonAsync<List<VehicleDto>>();

                if (vehicleDtos == null) return null;
                
                var vehicles = VehicleMapper.MapToVehicles(vehicleDtos);
                return vehicles?.Count > 0 ? vehicles[0] : null;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("The number plate was not found. Please check the registration number and try again.");
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("There was a problem connecting to the MOT service. Please try again later.");
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error occured: {ex.Message}");
                return null;
            }
        }
    }
}
