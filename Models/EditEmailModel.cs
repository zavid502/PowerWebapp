using System.ComponentModel.DataAnnotations;

namespace Models;

public class EditEmailModel
{
    [Required(ErrorMessage = "New email is required.")]
    public string Email { get; set; } = string.Empty;
}