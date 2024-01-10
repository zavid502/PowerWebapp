namespace Models;

public class TokensModel
{
    public string Token { get; set; } 
    public string RefreshToken { get; set; }

    public TokensModel()
    {
        Token = string.Empty;
        RefreshToken = String.Empty;
    }
}