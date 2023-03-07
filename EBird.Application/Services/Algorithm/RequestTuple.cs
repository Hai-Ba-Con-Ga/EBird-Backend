namespace EBird.Application.Services.Algorithm;
public class RequestTuple
{

	public Guid id { get; set; }
	public (double latitude, double longitude) location { get; set; }
	public int elo { get; set; }
	public DateTime date { get; set; }
	public bool isVip { get; set; }

	public RequestTuple(Guid id, (double latitude, double longitude) location, int elo, DateTime date, bool isVip)
	{
		this.id = id;
		this.location = location;
		this.elo = elo;
		this.date = date;
		this.isVip = isVip;
	}

	public RequestTuple()
	{
        this.id = Guid.Empty;
        this.location = (0, 0);
        this.elo = 0;
        this.date = DateTime.Now;
        this.isVip = false;
    }
	
	public override string ToString()
	{
        return $"location: {location}, elo: {elo}, date: {date}, isVip: {isVip}, id: {id}";
    }
}