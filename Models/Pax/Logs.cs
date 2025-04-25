using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsTodo.Models;

[Table("logs", Schema = "public")]
public class SysLogs
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    [JsonIgnore]
    public int Id { get; set; }

    [Required]
    [Column("timestamp")]
    [DataType(DataType.Time)]
    [JsonIgnore]
    public required DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [Column("message_text")]
    [JsonIgnore]
    public required string MessageText { get; set; }

    [Column("type")]
    [JsonIgnore]
    public required string Type { get; set; }

    [Column("phone_number")]
    [JsonIgnore]
    public string? UserPhone { get; set; }
}
