#region Imports
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace WhatsTodo.Models;
#endregion

[Table("whatsapp_bot_log", Schema = "public")]
public class WhatsappBotLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required, NotNull]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    [Required, NotNull]
    [StringLength(20)]
    [Column ("phone_number")]
    public string UserPhone { get; set; }

    [Required, NotNull]
    [StringLength (10)]
    [Column("direction")]
    [RegularExpression("^(incoming|outgoing)$")]
    public string Directrion { get; set; }

    [Required, NotNull]
    [Column("message_text")]
    public string MessageText { get; set; }
}
