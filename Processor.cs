using WhatsTodo.Database;

namespace WhatsTodo;

public class TaskCommand
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Time { get; set; }
    public bool Success { get; set; }
}

public static class Processor
{
    #region ParseTask

    private static TaskCommand ParseAddTaskCommand(ref string message)
    {
        var withoutCommand = message.Substring(message.IndexOf(' ') + 1);
        var time = withoutCommand.Substring(withoutCommand.Length - 5);
        var withoutTime = withoutCommand.Substring(0, withoutCommand.Length - 5).Trim();
        var title = withoutTime.Split(' ')[0];
        var description = withoutTime.Substring(title.Length).Trim();

        try
        {
            if (!TimeSpan.TryParse(time, out TimeSpan notificationTime))
                return new TaskCommand
                {
                    Success = false
                };
        }
        catch
        {
            return new TaskCommand
            {
                Success = false
            };
        }
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(time))
        {
            return new TaskCommand
            {
                Success = false
            };
        }

        return new TaskCommand
        {
            Title = title,
            Description = description,
            Time = time,
            Success = true
        };
    }

    private static TaskCommand ParseEditTaskCommand(ref string message)
    {
        var withoutCommand = message.Substring(message.IndexOf(' ') + 1);
        var time = withoutCommand.Substring(withoutCommand.Length - 5);
        var withoutTime = withoutCommand.Substring(0, withoutCommand.Length - 5).Trim();
        var title = withoutTime.Split(' ')[0];
        var description = withoutTime.Substring(title.Length).Trim();
        try
        {
            if (!TimeSpan.TryParse(time, out TimeSpan notificationTime))
                return new TaskCommand
                {
                    Success = false
                };
        }
        catch
        {
            return new TaskCommand
            {
                Success = false
            };
        }
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(time))
        {
            return new TaskCommand
            {
                Success = false
            };
        }

        return new TaskCommand
        {
            Title = title,
            Description = description,
            Time = time,
            Success = true
        };
    }

    #endregion

    #region ValidateTask

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

            if (string.IsNullOrEmpty(taskCommand.Title) || string.IsNullOrEmpty(taskCommand.Description))
                return false;

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

    #endregion

    public static void Handler(dynamic message)
    {
        if (!Database.Database.UserExists(message.User))
        {
            Database.Database.AddUser(message.User);
            Bot.SendMessageTextAsync(message.User, Resources.FirstUserMessage);
            return;
        }

        if (!message.Text.StartsWith("/"))
            return;

        string text = message.Text;
        string command = text.Split(' ')[0].ToLower();

        if (Commands.AddCommand.Split(",").Any(c => command.Contains(c.Trim())))
        {
            try
            {
                var taskCommand = ParseAddTaskCommand(ref text);

                if (!taskCommand.Success)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                    return;
                }

                taskCommand.Title = taskCommand?.Title?.Trim();
                var normalizedTitle = taskCommand?.Title?.ToLowerInvariant();
                bool TaskExistsResult = Database.Database.TaskExists(normalizedTitle, message.User);

                if (TaskExistsResult)
                {
                    Bot.SendMessageTextAsync(
                        message.User,
                        $"Já existe uma tarefa ativa com o título '{taskCommand?.Title}'. Por favor, escolha um título diferente ou edite a tarefa existente usando /edittask."
                    );
                }
                else if (!ValidateAndAddTask(taskCommand, message.User))
                {
                    Bot.SendMessageTextAsync(message.User, Resources.ErrorAddtask);
                }
                else
                {
                    Bot.SendMessageTextAsync(
                        message.User,
                        $"Task criada com sucesso!\nTítulo: {taskCommand?.Title}\nDescrição: {taskCommand?.Description}\nHorário: {taskCommand?.Time}"
                    );
                }
            
            }
            catch (Exception)
            {
                Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
            }
        }
        else if (Commands.EditCommand.Split(",").Any(c => command.Contains(c.Trim())))
        {
            var editTaskCommand = ParseEditTaskCommand(ref text);

            if (!editTaskCommand.Success)
            {
                Bot.SendMessageTextAsync(message.User, Resources.ErrorEdittask);
                return;
            }
            if (ValidateAndUpdateTask(editTaskCommand, message.User))
            {
                Bot.SendMessageTextAsync(
                    message.User,
                    $"Task atualizada com sucesso!\nTítulo: {editTaskCommand.Title}\nNova Descrição: {editTaskCommand.Description}\nNovo Horário: {editTaskCommand.Time}"
                );
                return;
            }
        }
        else if (Commands.ListCommand.Split(",").Any(c => command.Contains(c.Trim())))
        {
            List<dynamic> tasks = Database.Database.GetUserTasks(message.User);

            if (tasks.Count == 0)
            {
                Bot.SendMessageTextAsync(message.User, Resources.DontHaveTask);
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
        }
        else if (Commands.DeleteCommand.Split(",").Any(c => command.Contains(c.Trim())))
        {
            try
            {
                var taskTitle = message.Text.Substring(message.Text.IndexOf(' ') + 1).Trim();

                bool taskExistsAndBeRemoved = Database.Database.RemoveTask(taskTitle, message.User);

                if (!taskExistsAndBeRemoved)
                {
                    Bot.SendMessageTextAsync(
                        message.User,
                        "Tarefa não encontrada ou já foi concluída. Use /listtask para ver suas tarefas pendentes."
                    );
                }
                else
                {
                    Bot.SendMessageTextAsync(
                        message.User,
                        $"Tarefa '{taskTitle}' removida com sucesso!"
                    );
                }
            }
            catch (Exception)
            {
                Bot.SendMessageTextAsync(message.User, Resources.ErrorDeletetask);
            }
        }
        else if (command == "/creditos")
        {
            Bot.SendMessageTextAsync(
                message.User,
                "WhatsTodo - Desenvolvido pela equipe TC\n" +
                "Versão 0.0.1 Alpha\n\n" +
                "Github: https://github.com/MilyZani"
            );
        }
        else if (Commands.HelpCommand.Split(",").Any(c => command.Contains(c.Trim())))
        {
            Bot.SendMessageTextAsync(message.User, Resources.HelpMessageText);
        }
        else
        {
            Bot.SendMessageTextAsync(message.User, Resources.HelpMessageText);
        }
    }
}
