namespace Models;

public class StationExtendedModel : StationModel
{
    public int Slots { get; set; }
    public List<PowerBankModel> PowerBanks { get; set; } = new();
}