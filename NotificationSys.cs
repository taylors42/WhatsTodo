using System.Threading;

namespace WhatsTodo;

public static class NotificationSystem
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    public static void Start()
    {
        Task.Run(NotificationLoop, _cancellationTokenSource.Token);
    }

    public static void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    private static TimeZoneInfo GetBrasiliaTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        }
    }

    private static async Task NotificationLoop()
    {
        var brasiliaTimeZone = GetBrasiliaTimeZone();

        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                var nowInBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
                var notifications = Database.Database.GetPendingNotifications();

                foreach (var (userPhone, title, description) in notifications)
                {
                    var message = $"🔔 Lembrete!\n\n📌 *{title}*\n📝 {description}";
                    await Bot.SendMessageTextAsync(userPhone, message);
                    Database.Database.MarkTaskAsCompleted(title, userPhone);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), _cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no loop de notificações: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(10), _cancellationTokenSource.Token);
            }
        }
    }
}
