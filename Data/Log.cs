using Microsoft.EntityFrameworkCore;
using WhatsTodo.Models;

namespace WhatsTodo.Data;
public class Log
{
    public class DbContextFactory
    {
        public static PaxDbContext Create()
        {
            var options = new DbContextOptionsBuilder<PaxDbContext>();
            options.UseNpgsql(AppSettings.ConnectionString);
            return new PaxDbContext(options.Options);
        }
    }

    /// <summary>
    /// Add new log in the database
    /// </summary>
    /// <param name="phoneNumber">The Whatsapp Number of the user</param>
    /// <param name="description">The Direction of message, Incoming | Outgoing</param>
    /// <param name="messageText">The Text message of the user</param>
    public static async Task LogWhatsContactAsync(string phoneNumber, string description, string messageText)
    {
        using var context = DbContextFactory.Create();
        await context.AddAsync<WhatsappBotLog>(
            new WhatsappBotLog()
            {
                UserPhone = phoneNumber,
                Directrion = description,
                MessageText = messageText,
                Timestamp = DateTime.UtcNow.ToUniversalTime().AddHours(-3)
            }
        );
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Add new action log in the database
    /// </summary>
    /// <param name="phoneNumber">The Whatsapp Number of the user</param>
    /// <param name="description">The Direction of message, Incoming | Outgoing</param>
    /// <param name="messageText">The Text message of the user</param>
    public static async Task LogMessageAsync(string phoneNumber, string message, string type)
    {
        using var context = DbContextFactory.Create();

        var log = new SysLogs()
        {
            UserPhone = phoneNumber,
            MessageText = message,
            Type = type,
            Timestamp = DateTime.UtcNow.ToUniversalTime().AddHours(-3)
        };

        await context.AddAsync<SysLogs>(log);
        await context.SaveChangesAsync();
    }
}
