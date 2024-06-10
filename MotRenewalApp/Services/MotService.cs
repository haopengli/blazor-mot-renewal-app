﻿using Microsoft.Extensions.Options;
using MotRenewalApp.Models;

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
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.Url}{registrationNumber}");
                request.Headers.Add("x-api-key", _settings.ApiKey);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var vehicles = await response.Content.ReadFromJsonAsync<List<Vehicle>>();
                return vehicles?.Count > 0 ? vehicles[0] : null;
            }
            catch (HttpRequestException ex)
            {
                // Log or handle error
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log or handle error
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}