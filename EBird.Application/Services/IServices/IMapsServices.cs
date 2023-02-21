using EBird.Application.Model.Maps;

namespace EBird.Application.Services.IServices
{
    public interface IMapsServices
    {
        // Task<GeocodingApiLocation> GetGeocoding(AddressMap addressMap);
        Task<GeocodingApiLocation> GetGeocoding(string address);

    }
}