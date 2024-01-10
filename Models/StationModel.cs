namespace Models;

public class StationModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Location Location { get; set; } = new();

    public double DistanceTo(double latitude, double longitude)
    {
        return new GeoCoordinate(latitude, longitude).GetDistanceTo(new GeoCoordinate(Location.Latitude,
            Location.Longitude));
    }
}