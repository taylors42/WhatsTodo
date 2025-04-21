#region Imports
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
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
    [JsonIgnore]
    public int Id { get; set; }

    [Required, NotNull]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("timestamp")]
    [JsonIgnore]
    public required DateTime Timestamp { get; set; } = DateTime.UtcNow;
    [Required, NotNull]
    [StringLength(20)]
    [Column ("phone_number")]
    [JsonIgnore]
    public required string UserPhone { get; set; }

    [Required, NotNull]
    [StringLength (10)]
    [Column("direction")]
    [JsonIgnore]
    [RegularExpression("^(incoming|outgoing)$")]
    public required string Directrion { get; set; }

    [Required, NotNull]
    [Column("message_text")]
    [JsonIgnore]
    public required string MessageText { get; set; }
}
