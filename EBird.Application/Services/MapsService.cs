
using EBird.Application.Services.IServices;
using Newtonsoft.Json;

public class MapsService : IMapsServices
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";
    private const string ApiKey = "AIzaSyAY3ufqDLAZJz8dS54hHPWxUCl9pMnsKQE";

    public MapsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task GetAddress()
    {
        string address = "VQJJ+XGJ, khu B ký túc xá đại học quốc gia, Đông Hoà, Dĩ An, Bình Dương, Việt Nam";

        // Build the Geocoding API request URI with the address and API key
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

            double latitude = location.Lat;
            double longitude = location.Lng;

            Console.WriteLine($"Latitude: {latitude}, Longitude: {longitude}");
        }
        else
        {
            Console.WriteLine($"Geocoding request failed with status: {result.Status}");
        }
    }

    private class GeocodingApiResponse
    {
        public string Status { get; set; }
        public GeocodingApiResult[] Results { get; set; }
    }

    private class GeocodingApiResult
    {
        public GeocodingApiGeometry Geometry { get; set; }
    }

    private class GeocodingApiGeometry
    {
        public GeocodingApiLocation Location { get; set; }
    }

    private class GeocodingApiLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
