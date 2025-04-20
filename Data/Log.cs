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
    public static async Task LogInformation(string phoneNumber, string description, string messageText)
    {
        var context = DbContextFactory.Create();
        await context.AddAsync<WhatsappBotLog>(
            new WhatsappBotLog()
            {
                UserPhone = phoneNumber,
                Directrion = description,
                MessageText = messageText
            }
        );
        await context.SaveChangesAsync();
    }
}
