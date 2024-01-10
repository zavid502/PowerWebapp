using System.ComponentModel.DataAnnotations;

namespace Models;

public class EditUsernameModel
{
    [Required(ErrorMessage = "New username is required.")]
    public string NewName { get; set; } = string.Empty;
}