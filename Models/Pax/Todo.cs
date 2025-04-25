using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsTodo.Models;

[Table("todos", Schema = "public")]
public class Todo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    [JsonIgnore]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("title")]
    [JsonIgnore]
    public required string Title { get; set; }

    [Column("description")]
    [JsonIgnore]
    public required string Description { get; set; }

    [Required]
    [Column("notification_date")]
    [DataType(DataType.Time)]
    [JsonIgnore]
    public required DateTime NotificationDate { get; set; }

    [Column("is_completed")]
    [JsonIgnore]
    public required bool IsCompleted { get; set; } = false;

    [Column("created_at")]
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("completed_at")]
    [JsonIgnore]
    public DateTime? CompletedAt { get; set; }

    [Column("user_phone")]
    [JsonIgnore]

    public required string UserPhone { get; set; }

    [ForeignKey("UserPhone")]
    [JsonIgnore]
    public virtual User User { get; set; }
}




