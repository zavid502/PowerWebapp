namespace Models;

public interface IStationModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Location Location { get; set; }
}