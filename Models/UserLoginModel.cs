using System.ComponentModel.DataAnnotations;

namespace Models;

public class UserLoginModel
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    public UserLoginModel()
    {
        Username = string.Empty;
        Password = string.Empty;
    }
}