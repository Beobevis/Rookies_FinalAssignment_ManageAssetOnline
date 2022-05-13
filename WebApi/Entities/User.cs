namespace WebApi.Entities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User : BaseEntity
{
    [Required]
    public string StaffCode {get; set;} = null!;
    [Required]
    public string Firstname { get; set; } = null!;
    [Required]
    public string Lastname { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [JsonIgnore]
    public string? PasswordHash { get; set; }
    [Required]
    public DateTime JoinDate {get; set;}
    [Required]
    public Role Type { get; set; }
    [Required]
    public DateTime DoB {get; set;}
    [Required]
    public string? Gender {get; set;}
    public bool IsDisabled {get; set;}
    [Required]
    public bool IsFirstLogin {get; set;}
    [Required]
    public string Location {get; set;} = null!;
    public DateTime CreateAt { get; set; }
    public int CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? UpdateBy { get; set; }
    [JsonIgnore]
    public ICollection<Assignment>? AssignTos {get; set;}
    [JsonIgnore]
    public ICollection<Assignment>? AssignBys {get; set;}
    [JsonIgnore]
    public ICollection<ReturnRequest>? ReturnBys {get; set;}
    [JsonIgnore]
    public ICollection<ReturnRequest>? AcceptBys {get; set;}
}