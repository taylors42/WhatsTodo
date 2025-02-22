using System.Threading;

namespace WhatsTodo;

public static class NotificationSystem
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    private const int CHECK_INTERVAL_SECONDS = 10;

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
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }
            catch
            {
                return TimeZoneInfo.CreateCustomTimeZone(
                    "Brasilia Standard Time",
                    TimeSpan.FromHours(-3),
                    "Brasilia Time",
                    "Brasilia Standard Time"
                );
            }
        }
    }

    private static async Task NotificationLoop()
    {
        var brasiliaTimeZone = GetBrasiliaTimeZone();

        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                var nowInBrasilia = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    brasiliaTimeZone
                );
                var tasks = Database.Database.GetTasksDueAt(nowInBrasilia);

                foreach (var task in tasks)
                {
                    var formattedTime = nowInBrasilia.ToString("dd/MM/yyyy HH:mm");
                    var message = $"🔔 Tarefa Agendada!\n\n📌 *{task.Title}*\n📝 {task.Description}\n⏰ {formattedTime}";
                    await Bot.SendMessageTextAsync(task.UserPhone, message);
                    await Database.Database.MarkNotificationSent(task.Id, nowInBrasilia);
                }

                var nextCheck = nowInBrasilia.AddSeconds(
                    CHECK_INTERVAL_SECONDS - (nowInBrasilia.Second % CHECK_INTERVAL_SECONDS)
                );
                var delayMs = (int)(nextCheck - nowInBrasilia).TotalMilliseconds;
                await Task.Delay(delayMs, _cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no loop de notificações: {ex.Message}");
                await Task.Delay(
                    TimeSpan.FromSeconds(CHECK_INTERVAL_SECONDS),
                    _cancellationTokenSource.Token
                );
            }
        }
    }
}
