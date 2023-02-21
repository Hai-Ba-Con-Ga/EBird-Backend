using EBird.Application.Services;
using GoogleApi;
using GoogleApi.Entities.Maps.Geocoding;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;

public class MapsService : IMapsServices
{
    public MapsService()
    {
    }
    public async Task GetAddress()
    {
        string apiKey = "";
        string address = "1600 Amphitheatre Parkway, Mountain View, CA";
        // Make the geocoding request
        var response = await GoogleApi.GoogleMaps.Geocode.AddressGeocode.QueryAsync(new AddressGeocodeRequest
        {
            Key = apiKey,
            Address = address
        });
        // Check if the geocoding request was successful
        if (response.Status == GoogleApi.Entities.Common.Enums.Status.Ok)
        {
            // Get the latitude and longitude of the geocoded address
            var location = response.Results.FirstOrDefault()?.Geometry?.Location;

            if (location != null)
            {
                double latitude = location.Latitude;
                double longitude = location.Longitude;

                Console.WriteLine($"Latitude: {latitude}, Longitude: {longitude}");
            }
            else
            {
                Console.WriteLine("Could not retrieve location information for the address.");
            }
        }
        else
        {
            Console.WriteLine($"Geocoding request failed with status: {response.Status}");
        }
    }
}
