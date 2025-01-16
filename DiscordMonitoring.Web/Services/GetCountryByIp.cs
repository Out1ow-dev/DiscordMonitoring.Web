using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;

namespace DiscordMonitoring.Web.Services
{
    public static class GetCountryByIp
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetCountryAsync(string ip)
        {
            try
            {
                var response = await httpClient.GetStringAsync($"https://api.sypexgeo.net/json/{ip}");
                var json = JObject.Parse(response);
                return json["country"]?["name_ru"]?.ToString();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
                return null;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Parsing Error: {jsonEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }

}
