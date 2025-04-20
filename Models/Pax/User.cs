using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatsTodo.Models;

[Table("users", Schema = "public")]
public class User
{
    [Key]
    [Column("phone")]
    [StringLength(20)]
    public string Phone { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<Todo> Todos { get; set; }
}

