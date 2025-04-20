using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WhatsTodo.Models;

namespace WhatsTodo.Data;

/// <summary>
/// Class responsible for handle todo actions
/// </summary>
public class TodoData
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
    /// verifys if some tasks exists
    /// </summary>
    /// <param name="title">The title of the task</param>
    /// <param name="phoneNumber">The Whatsapp phone number of the user</param>
    /// returns a Task<bool> if this task already exists
    public static async Task<bool> TaskExistsAsync(string title, string phoneNumber)
    {
        using var context = DbContextFactory.Create();
        return await context.Todos
            .AnyAsync(task => task.Title.Contains(title) && task.UserPhone == phoneNumber);
    }

    /// <summary>
    /// Add a todo task on database
    /// </summary>
    /// <param name="title">The name of the task</param>
    /// <param name="description">The description of the task</param>
    /// <param name="notificationDate">The notification Date</param>
    /// <param name="phoneNumber">The Whatsapp phone number of the user</param>
    public static async Task AddTaskAsync(string title, string description, DateTime notificationDate, string phoneNumber)
    {
        using var ctx = DbContextFactory.Create();
        await ctx.AddAsync(new Todo(){
                Title = title,
                Description = description,
                NotificationDate = notificationDate,
                UserPhone = phoneNumber
            }
        );
        await ctx.SaveChangesAsync();
    }

    /// <summary>
    /// Edit a todo task on database
    /// </summary>
    /// <param name="title">The name of the task</param>
    /// <param name="description">The description of the task</param>
    /// <param name="notificationDate">The notification Date</param>
    /// <param name="phoneNumber">The Whatsapp phone number of the user</param>
    public static async Task EditTaskAsync(string title, string description, DateTime notificationDate, string phoneNumber)
    {
        using var ctx = DbContextFactory.Create();
        var task = await ctx
            .Todos
            .FirstOrDefaultAsync(task => 
                task.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && 
                task.UserPhone == phoneNumber
            );

        task!.Description = description;
        task.NotificationDate = notificationDate;
        task.IsCompleted = false;
        task.CompletedAt = null;

        await ctx.SaveChangesAsync();
    }

    /// <summary>
    /// Get all to-do tasks and return a IEnumerable
    /// </summary>
    /// <param name="now">the brazilian actual time</param>
    /// returns a IEnumerable with all <todos>
    public static IEnumerable<Todo> GetTodo(DateTime now)
    {
        using var ctx = DbContextFactory.Create();
        return [.. ctx
            .Todos.Where(t =>
                t.IsCompleted == false
                && t.CompletedAt == null
                && t.NotificationDate < now
            )];
    }

    /// <summary>
    /// Mark a todo as completed and Notify the User
    /// </summary>
    /// <param name="id">The id of the task </param>
    /// <param name="msg">The message of the notification</param>
    /// <param name="now">The brazilian actual time</param>
    public static async Task CompleteTodoAndNotifyUserAsync(int id, string msg, DateTime now)
    {
        using var ctx = DbContextFactory.Create();
        var todo = await ctx.Todos.FirstOrDefaultAsync(t => t.Id == id);
        todo!.CompletedAt = now;
        todo.IsCompleted = true;
        await ctx.SaveChangesAsync();
        await Bot.SndMsg(todo.UserPhone, msg);
    }

    /// <summary>
    /// Mark a todo as completed and Notify the User
    /// </summary>
    /// <param name="phoneNumber">The user phoneNumber </param>
    public static async Task GetAndNotifyPendingTasksAsync(string phoneNumber)
    {
        using var ctx = DbContextFactory.Create();
        string? taskList = "Suas tarefas pendentes:\n\n";

        var todos = ctx.Todos.Where(t => t.IsCompleted == false && t.UserPhone == phoneNumber);

        if (todos.Any() is false)
        {
            await Bot.SndMsg(phoneNumber, Resources.DontHaveTask);
            return;
        }

        foreach (var todo in todos)
        {
            taskList += $"📌 *{todo.Title}*\n";
            taskList += $"📝 {todo.Description}\n";
            taskList +=
            $"⏰ {todo.NotificationDate:dd/MM/yyyy} às {todo.NotificationDate:HH:mm}\n\n";
        }
        await Bot.SndMsg(phoneNumber,  taskList);
    }


    /// <summary>
    /// Delete a task
    /// </summary>
    /// <param name="title">Title of the task</param>
    /// <param name="phoneNumber">The user phoneNumber </param>
    public static async Task RemoveTaskAsync(string title, string phoneNumber)
    {
        using var ctx = DbContextFactory.Create();
        var todo = await ctx
            .Todos
            .FirstOrDefaultAsync(task => 
                task.Title == title && 
                task.UserPhone == phoneNumber
            );
        ctx.Todos.Remove(todo!);
        await ctx.SaveChangesAsync();
    }
}
