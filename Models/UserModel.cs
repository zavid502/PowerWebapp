namespace Models;

public class UserModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string RegisterDate { get; set; }
    public int Role { get; set; }
    
    public UserModel()
    {
        Username = string.Empty;
        Email = string.Empty;
        RegisterDate = string.Empty;
        Role = 1;
    }

    public string FormattedRegisterDate()
    {
        if (RegisterDate == string.Empty)
        {
            return string.Empty;
        }
        DateTime dateTime = DateTime.ParseExact(RegisterDate, "yyyy-MM-ddTHH:mm:ss.fffZ", null, System.Globalization.DateTimeStyles.RoundtripKind);
        string betterDate = $"{dateTime.Date}, {dateTime.TimeOfDay}";

        return betterDate;
    }
}