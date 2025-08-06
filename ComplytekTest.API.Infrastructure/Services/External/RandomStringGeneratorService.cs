using ComplytekTest.API.Application.Services.External;
using ComplytekTest.API.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace ComplytekTest.API.Infrastructure.Services.External
{
    public class RandomStringGeneratorService : IRandomStringGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly ILogger<RandomStringGeneratorService> _logger;

        public RandomStringGeneratorService(HttpClient httpClient, IConfiguration configuration, ILogger<RandomStringGeneratorService> logger)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["RandomStringApiUrl"] ?? throw new ArgumentNullException("RandomStringApiUrl not found.");
            _logger = logger;
        }

        public async Task<string> GenerateAsync()
        {
            try
            {
                var requestBody = new RandomCodeRequest
                {
                    CodesToGenerate = 1,
                    OnlyUniques = true,
                    CharactersSets = Enumerable.Repeat("\\d\\l\\L\\@", 8).ToArray()
                };

                var response = await _httpClient.PostAsJsonAsync(_apiUrl, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Codito API returned status code {StatusCode}", response.StatusCode);
                    throw new Exception("Failed to get random string from Codito API.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var codes = JsonSerializer.Deserialize<string[]>(responseContent);
                var code = codes?.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(code))
                {
                    _logger.LogWarning("Codito API returned an empty or null code.");
                    throw new Exception("Codito API returned an empty code.");
                }

                return code;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating random string from Codito API.");
                throw new Exception("Something went wrong while generating random string.", ex);
            }
        }
    }
}