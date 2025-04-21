using Microsoft.EntityFrameworkCore;
using WhatsTodo.Models;

namespace WhatsTodo.Data;

/// <summary>
/// Class responsible for handle user actions
/// </summary>
public class UserData
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
    /// Check if the user is on the database
    /// </summary>
    /// <param name="userPhone">The Whatsapp Number of the user</param>
    public static async Task<bool> UserExists(string userPhone)
    {
        var ctx = DbContextFactory.Create();
        return await ctx.User.AnyAsync(u => u.Phone == userPhone);
    }

    /// <summary>
    /// Add a userPhone in the database
    /// </summary>
    /// <param name="userPhone">The Whatsapp Number of the user</param>
    public static async Task AddUser(string userPhone)
    {
        using var context = DbContextFactory.Create();

        var user = new User()
        {
            Phone = userPhone
        };

        await context.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
