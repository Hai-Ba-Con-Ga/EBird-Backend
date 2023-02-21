
using EBird.Application.AppConfig;
using EBird.Application.Exceptions;
using EBird.Application.Model.Maps;
using EBird.Application.Services.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class MapsService : IMapsServices
{
    private readonly HttpClient _httpClient;
    //private readonly static IConfiguration _configuration;
    private const string ApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
    private const string ApiKey = "AIzaSyAY3ufqDLAZJz8dS54hHPWxUCl9pMnsKQE";
    
    //GoogleSetting google = _configuration.GetSection(GoogleSetting.GoogleSettingString).Get<GoogleSetting>();

    public MapsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // public async Task<GeocodingApiLocation> GetGeocoding(AddressMap addressMap)
    public async Task<GeocodingApiLocation> GetGeocoding(string address)

    {

        // Build the Geocoding API request URI with the address and API key
        // var uri = new Uri($"{ApiUrl}?address={Uri.EscapeDataString(addressMap.Address.Trim())}&key={ApiKey}");
        var uri = new Uri($"{ApiUrl}?address={Uri.EscapeDataString(address)}&key={ApiKey}");


        // Send the Geocoding API request
        var response = await _httpClient.GetAsync(uri);

        // Parse the response JSON
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GeocodingApiResponse>(json);

        // Check if the Geocoding API request was successful
        if (result.Status == "OK")
        {
            // Get the latitude and longitude of the geocoded address
            var location = result.Results[0].Geometry.Location;
            return location;
        }
        else
        {
            throw new BadRequestException($"Geocoding request failed with status: {result.Status}");
        }
    }

}
