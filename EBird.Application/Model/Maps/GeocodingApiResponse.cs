namespace EBird.Application.Model.Maps;

public class GeocodingApiResponse
{
    public string Status { get; set; }
    public GeocodingApiResult[] Results { get; set; }
}