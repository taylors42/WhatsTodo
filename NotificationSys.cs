using System;
using System.Threading;
using System.Threading.Tasks;
using WhatsTodo.Data;
using WhatsTodo.Models;

namespace WhatsTodo;

public static class NotificationSystem
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    private const int CHECK_INTERVAL_SECONDS = 5;
    public static void Start()
    {
        Task.Run(RunNotificationLoop, _cancellationTokenSource.Token);
    }

    public static void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    private static async Task RunNotificationLoop()
    {
        Console.WriteLine("Sistema de notificação iniciado");
        
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                var datetime = DateTime.UtcNow.ToUniversalTime().AddHours(-3);
                foreach (var todo in TodoData.GetTodo(datetime))
                {
                    await TodoData.CompleteTodoAndNotifyUserAsync(todo.Id, Fmt(todo.Title, todo.Description, datetime), datetime);
                }
                await Task.Delay(TimeSpan.FromSeconds(CHECK_INTERVAL_SECONDS), _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("exception no notificationsys");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no sistema de notificação: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                await Task.Delay(TimeSpan.FromSeconds(CHECK_INTERVAL_SECONDS), _cancellationTokenSource.Token);
            }
        }

        Console.WriteLine("Sistema de notificação finalizado");
    }
    private static string Fmt(string title, string description, DateTime time) =>
        $"🔔 Lembrete! 📅 {time:dd/MM/yyyy HH:mm}\n\n📌 *{title}*\n\n{description}";
    
}