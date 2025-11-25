using System.ComponentModel.DataAnnotations;

namespace Logistics.Logs.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}