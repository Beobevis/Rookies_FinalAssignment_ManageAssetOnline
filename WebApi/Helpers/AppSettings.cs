using System.ComponentModel.DataAnnotations;

namespace WebApi.Helpers;

public class AppSettings
{
    //[Required]
    public string Secret { get; set; } = null!;
}