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
                return new TaskCommand { Success = false };
        }
        catch
        {
            return new TaskCommand { Success = false };
        }
        if (
            string.IsNullOrEmpty(title)
            || string.IsNullOrEmpty(description)
            || string.IsNullOrEmpty(time)
        )
        {
            return new TaskCommand { Success = false };
        }

        return new TaskCommand
        {
            Title = title,
            Description = description,
            Time = time,
            Success = true,
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
                return new TaskCommand { Success = false };
        }
        catch
        {
            return new TaskCommand { Success = false };
        }
        if (
            string.IsNullOrEmpty(title)
            || string.IsNullOrEmpty(description)
            || string.IsNullOrEmpty(time)
        )
        {
            return new TaskCommand { Success = false };
        }

        return new TaskCommand
        {
            Title = title,
            Description = description,
            Time = time,
            Success = true,
        };
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

        string text = message.Text;
        string command = text.Split(' ')[0].ToLower();

        if (Commands.AddCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            try
            {
                var taskCommand = ParseAddTaskCommand(ref text);

                if (!taskCommand.Success)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                    return;
                }

                #region NormalizeCommand

                taskCommand.Title = taskCommand?.Title?.Trim();
                var normalizedTitle = taskCommand?.Title?.ToLowerInvariant();

                #endregion

                bool TaskExistsResult = Database.Database.TaskExists(normalizedTitle, message.User);
                if (TaskExistsResult)
                {
                    Bot.SendMessageTextAsync(
                        message.User,
                        $"Já existe uma tarefa ativa com o título '{taskCommand?.Title}'. Por favor, escolha um título diferente ou edite a tarefa existente usando /edittask."
                    );
                    return;
                }
                ValidadeStatus status = ValidationFlow.ValidateAndAddTask(taskCommand, message.User);
                if (status.CatchError)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.ErrorAddtask);
                }
                else if (status.SintaxeError)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
                }
                else if (status.TimeError)
                {
                    Bot.SendMessageTextAsync(message.User, Resources.TimeInvalid);
                }
                else if (status.TaskResult)
                {
                    Bot.SendMessageTextAsync(
                        message.User,
                        $"Task criada com sucesso!\nTítulo: {taskCommand?.Title}\nDescrição: {taskCommand?.Description}\nHorário: {taskCommand?.Time}"
                    );
                }
                else
                {
                    Console.WriteLine("Out of range");
                }
            }
            catch (Exception)
            {
                Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
            }
        }
        else if (Commands.EditCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            TaskCommand editTaskCommand = ParseEditTaskCommand(ref text);

            if (!editTaskCommand.Success)
            {
                Bot.SendMessageTextAsync(message.User, Resources.ErrorEdittask);
                return;
            }

            ValidadeStatus status = ValidationFlow.ValidateAndUpdateTask(editTaskCommand, message.User);
            
            if (status.TaskResult)
            {
                Bot.SendMessageTextAsync(
                    message.User,
                    $"Task atualizada com sucesso!\nTítulo: {editTaskCommand.Title}\nNova Descrição: {editTaskCommand.Description}\nNovo Horário: {editTaskCommand.Time}"
                );
            }
            else if (status.CatchError)
            {
                Bot.SendMessageTextAsync(message.User, Resources.ErrorEdittask);
            }
            else if (status.SintaxeError)
            {
                Bot.SendMessageTextAsync(message.User, Resources.FormatInvalid);
            }
            else if (status.TimeError)
            {
                Bot.SendMessageTextAsync(message.User, Resources.TimeInvalid);
            }
            else
            {
                Console.WriteLine("Out of range");
            }
        }
        else if (Commands.ListCommand.Split(",").Select(c => c.Trim()).Contains(command))
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
                taskList +=
                    $"⏰ {task.NotificationDate:dd/MM/yyyy} às {task.NotificationTime:hh\\:mm}\n\n";
            }

            Bot.SendMessageTextAsync(message.User, taskList);
        }
        else if (Commands.DeleteCommand.Split(",").Select(c => c.Trim()).Contains(command))
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
                "WhatsTodo - Desenvolvido pela equipe TC\n"
                    + "Versão 0.0.1 Alpha\n\n"
                    + "Github: https://github.com/MilyZani"
            );
        }
        else if (Commands.HelpCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            Bot.SendMessageTextAsync(message.User, Resources.HelpMessageText);
        }
        else
        {
            Bot.SendMessageTextAsync(message.User, Resources.HelpMessageText);
        }
    }
}
