using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WhatsTodo.Models;

[Table("users", Schema = "public")]
public class User
{
    [Key]
    [Column("phone")]
    [StringLength(20)]
    [JsonIgnore]
    public string Phone { get; set; }

    [Column("created_at")]
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToUniversalTime();
}

