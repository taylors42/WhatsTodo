using WhatsTodo.Database;
namespace WhatsTodo;

public class TaskCommand
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Time { get; set; }
}

public static class Processor
{
    private static Dictionary<string, ContextState> Chats { get; set; } = [];

    public static TaskCommand ParseAddTaskCommand(ref string message)
    {
        var withoutCommand = message.Substring(message.IndexOf(' ') + 1);
        var time = withoutCommand.Substring(withoutCommand.Length - 5);
        var withoutTime = withoutCommand.Substring(0, withoutCommand.Length - 5).Trim();
        var title = withoutTime.Split(' ')[0];
        var description = withoutTime.Substring(title.Length).Trim();

        return new TaskCommand
        {
            Title = title,
            Description = description,
            Time = time
        };
    }

    private static bool ValidateAndAddTask(TaskCommand taskCommand, string userPhone)
    {
        try
        {
            if (!TimeSpan.TryParse(taskCommand.Time, out TimeSpan notificationTime))
                return false;

            var notificationDate = DateTime.Today;

            if (notificationTime < DateTime.Now.TimeOfDay)
                notificationDate = notificationDate.AddDays(1);

            return Database.Database.AddTask(
                taskCommand.Title,
                taskCommand.Description,
                notificationDate,
                notificationTime,
                userPhone
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao validar e adicionar tarefa: {ex.Message}");
            return false;
        }
    }

    private static bool ValidateAndUpdateTask(TaskCommand taskCommand, string userPhone)
    {
        try
        {
            if (!TimeSpan.TryParse(taskCommand.Time, out TimeSpan notificationTime))
                return false;

            var notificationDate = DateTime.Today;

            if (notificationTime < DateTime.Now.TimeOfDay)
                notificationDate = notificationDate.AddDays(1);

            return Database.Database.UpdateTask(
                taskCommand.Title,
                taskCommand.Description,
                notificationDate,
                notificationTime,
                userPhone
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao validar e atualizar tarefa: {ex.Message}");
            return false;
        }
    }

    private static TaskCommand ParseEditTaskCommand(ref string message)
    {
        var withoutCommand = message.Substring(message.IndexOf(' ') + 1);
        var time = withoutCommand.Substring(withoutCommand.Length - 5);
        var withoutTime = withoutCommand.Substring(0, withoutCommand.Length - 5).Trim();
        var title = withoutTime.Split(' ')[0];
        var description = withoutTime.Substring(title.Length).Trim();

        return new TaskCommand
        {
            Title = title,
            Description = description,
            Time = time
        };
    }

    public static void ProcessorHandler(dynamic message)
    {
        if (!Chats.ContainsKey(message.User))
        {
            Chats[message.User] = ContextState.InContext;
            Bot.SendMessageTextAsync(message.User, Resources.FirstUserMessage);
            return;
        }

        if (!message.Text.StartsWith("/")) return;

        string text = message.Text;
        var command = text.Split(' ')[0].ToLower();

        switch (command)
        {
            case "/addtask":
                try
                {
                    var taskCommand = ParseAddTaskCommand(ref text);

                    taskCommand.Title = taskCommand?.Title?.Trim();
                    var normalizedTitle = taskCommand?.Title?.ToLowerInvariant();

                    if (Database.Database.TaskExists(normalizedTitle, message.User))
                    {
                        Bot.SendMessageTextAsync(
                            message.User,
                            $"Já existe uma tarefa ativa com o título '{taskCommand?.Title}'. Por favor, escolha um título diferente ou edite a tarefa existente usando /edittask."
                        );
                    }
                    else if (!ValidateAndAddTask(taskCommand, message.User))
                    {
                        Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                    }
                    else
                    {
                        Bot.SendMessageTextAsync(
                            message.User,
                            $"Task criada com sucesso!\nTítulo: {taskCommand?.Title}\nDescrição: {taskCommand?.Description}\nHorário: {taskCommand?.Time}"
                        );
                    }
                    break;
                }
                catch (Exception)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                }
                break;

            case "/edittask":
                try
                {
                    var taskCommand = ParseEditTaskCommand(ref text);

                    if (ValidateAndUpdateTask(taskCommand, message.User))
                    {
                        Bot.SendMessageTextAsync(
                            message.User,
                            $"Task atualizada com sucesso!\nTítulo: {taskCommand.Title}\nNova Descrição: {taskCommand.Description}\nNovo Horário: {taskCommand.Time}"
                        );
                        return;
                    }
                    Bot.SendMessageTextAsync(
                        message.User,
                        "Tarefa não encontrada ou formato inválido. Use /ajuda para mais informações."
                    );
                    break;
                }
                catch (Exception)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                }
                break;

            case "/listtask":
                List<dynamic> tasks = Database.Database.GetUserTasks(message.User);
                if (tasks.Count == 0)
                {
                    Bot.SendMessageTextAsync(message.User, "Você não possui tarefas pendentes.");
                    return;
                }

                string? taskList = "Suas tarefas pendentes:\n\n";
                foreach (var task in tasks)
                {
                    taskList += $"📌 *{task.Title}*\n";
                    taskList += $"📝 {task.Description}\n";
                    taskList += $"⏰ {task.NotificationDate:dd/MM/yyyy} às {task.NotificationTime:hh\\:mm}\n\n";
                }

                Bot.SendMessageTextAsync(message.User, taskList);
                break;

            case "/deletetask":
                try
                {
                    var taskTitle = message.Text.Substring(message.Text.IndexOf(' ') + 1).Trim();

                    if (Database.Database.RemoveTask(taskTitle, message.User))
                    {
                        Bot.SendMessageTextAsync(
                            message.User,
                            $"Tarefa '{taskTitle}' removida com sucesso!"
                        );
                        return;
                    }
                    Bot.SendMessageTextAsync(
                        message.User,
                        "Tarefa não encontrada ou já foi concluída. Use /listtask para ver suas tarefas pendentes."
                    );
                    break;
                }
                catch (Exception)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                }
                break;

            case "/creditos":
                Bot.SendMessageTextAsync(
                    message.User,
                    "WhatsTodo - Desenvolvido pela equipe TC\n" +
                    "Versão 0.0.1 Alpha\n\n" +
                    "Github: https://github.com/MilyZani"
                );
                break;

            case "/help":
                Bot.SendMessageTextAsync(message.User, Resources.HelpMessageText);
                break;

            default:
                break;
        }
    }
}