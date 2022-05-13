using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities;

public class TokenLogout{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id {get; set;}
    public DateTime ExpirationDate {get; set;}
    public string Token {get; set;} = null!;
}