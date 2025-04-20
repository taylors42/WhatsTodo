using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsTodo.Models;

[Table("todos", Schema = "public")]
public class Todo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Required]
    [Column("notification_date")]
    [DataType(DataType.Time)]
    public DateTime NotificationDate { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Column("user_phone")]
    public string UserPhone { get; set; }

    [ForeignKey("UserPhone")]
    public virtual User User { get; set; }
}




