namespace EBird.Application.Services.Algorithm;
public class RequestTuple
{
	public (double latitude, double longitude) location { get; set; }
	public int elo { get; set; }
	public DateTime date { get; set; }
	public bool isVip { get; set; }

	public RequestTuple((double latitude, double longitude) location, int elo, DateTime date, bool isVip)
	{
		this.location = location;
		this.elo = elo;
		this.date = date;
		this.isVip = isVip;
	}
	
	public override string ToString()
	{
		return $"location: {location}, elo: {elo}, date: {date}, isVip: {isVip}";
	}
}