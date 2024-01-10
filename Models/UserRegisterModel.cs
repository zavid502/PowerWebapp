using System.ComponentModel.DataAnnotations;

namespace Models;

public class UserRegisterModel
{
    [Required(ErrorMessage = "Field is required.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    public string Password { get; set; }

    public UserRegisterModel()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }
}