using WhatsTodo.Database;

namespace WhatsTodo;

public static class Events
{
    private static bool _isRunning = false;
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);

    public static void NotificationSys()
    {
        if (_isRunning) return;
        _isRunning = true;

        Task.Run(static async () =>
        {
            while (true)
            {
                try
                {
                    var pendingNotifications = Database.Database.GetPendingNotifications();

                    foreach (var notification in pendingNotifications)
                    {
                        var message = $"🔔 Lembrete!\n\n" +
                                    $"Tarefa: {notification.Title}\n" +
                                    $"Descrição: {notification.Description}";

                        await Bot.SendMessageTextAsync(notification.UserPhone, message);
                        Database.Database.MarkTaskAsCompleted(notification.Title, notification.UserPhone);
                    }
                }
                catch (Exception ex)

                {
                    Console.WriteLine($"Erro no sistema de notificações: {ex.Message}");
                }

                await Task.Delay(CheckInterval);
            }
        });
    }
}
