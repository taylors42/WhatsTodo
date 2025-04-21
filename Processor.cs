using Microsoft.EntityFrameworkCore;
using WhatsTodo.Models;
using WhatsTodo.Data;
using System.Threading.Tasks;

namespace WhatsTodo;

public static class Processor
{
    #region ParseTask

    private static (string, string, DateTime) ParseTask(ref string message)
    {
        try
        {
            var withoutCommand = message.Substring(message.IndexOf(' ') + 1);
            var time = withoutCommand.Substring(withoutCommand.Length - 5);
            var withoutTime = withoutCommand.Substring(0, withoutCommand.Length - 5).Trim();
            var title = withoutTime.Split(' ')[0];
            var description = withoutTime.Substring(title.Length).Trim();
            var date = DateTime.Parse(time).ToUniversalTime().AddHours(-3);
            return (title, description, date);
        }
        catch
        {
            throw new Exception();
        }
    }

    #endregion

    public static async Task Handler(dynamic message)
    {
        #region VerifyUser

        string user = message.User;

        try
        {
            await Log.LogMessageAsync(user, "verify if user exists", "validation");
            if (await UserData.UserExists(user) is false)
            {
                await UserData.AddUser(user);
                await Bot.SndMsg(user, Resources.FirstUserMessage);
                await Log.LogMessageAsync(user, "user added on database", "validation");
                return;
            }
            await Log.LogMessageAsync(user, "user exists in the database", "validation");
        }
        catch
        {
            await Log.LogMessageAsync(user, "Catch na verificação de erro", "catch error");
            await Bot.SndMsg(user, "Erro na verificação");
        }

        #endregion
        
        string text = message.Text;
        string command = text.Split(' ')[0].ToLower();

        await Log.LogMessageAsync(user, $"user type a command | command: {command}", "task_action");

        #region AddTask

        if (Commands.AddCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            try
            {
                var (title, description, notificationDate) = ParseTask(ref text);

                if (await TodoData.TaskExistsAsync(title, user))
                {
                    await Bot.SndMsg(
                        user,
                        $"Já existe uma tarefa ativa com o título '{title}'. Por favor, escolha um título diferente ou edite a tarefa existente usando /edittask."
                    );
                    await Log.LogMessageAsync(user, $"exists a task with this name | title: {title}", "task_action");
                    return;
                }

                var currentHour = DateTime.UtcNow.ToUniversalTime().AddHours(-3);
                
                if (notificationDate < currentHour)
                {
                    await Bot.SndMsg(user, "horario no passado");
                    await Log.LogMessageAsync(user, $"this task is in the past | cpu time: {currentHour} | task time: {notificationDate}", "task_action");
                    return;
                }

                await TodoData.AddTaskAsync(
                    title,
                    description,
                    notificationDate,
                    user
                );

                await Bot.SndMsg(
                    user,
                    $"Task criada com sucesso!\nTítulo: {title}\nDescrição: {description}\nHorário: {notificationDate}"
                );

                await Log.LogMessageAsync(user, $"task added with success | title: {title}", "task_action");

                return;
            }
            catch
            {
                await Log.LogMessageAsync(user, "erro catch ao adicionar comando", "validation");
                await Bot.SndMsg(user, Resources.FormatInvalid);
            }
        }
        #endregion

        #region EditTask

        else if (Commands.EditCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            try
            {
                var (title, description, notificationDate) = ParseTask(ref text);

                var currentHour = DateTime.UtcNow.ToUniversalTime().AddHours(-3);

                if (notificationDate < currentHour)
                {
                    await Log.LogMessageAsync(user, $"this task is in the past | cpu time: {currentHour} | task time: {notificationDate}", "task_action");
                    await Bot.SndMsg(user, "Horario no passado");
                    return;
                }

                if (await TodoData.TaskExistsAsync(title, user) is false)
                {
                    await Log.LogMessageAsync(user, $"this task already exists | title: {title}", "task_action");
                    await Bot.SndMsg(user, Resources.TaskNotFound);
                    return;
                }

                await TodoData.EditTaskAsync(
                    title,
                    description,
                    notificationDate,
                    user
                );

                await Bot.SndMsg(
                    user,
                    $"Task atualizada com sucesso!\nTítulo: {title}\nNova Descrição: {description}\nNovo Horário: {notificationDate.Hour}:{notificationDate.Minute}"
                );

                await Log.LogMessageAsync(user, "task updated", "task_action");

                return;
            }
            catch
            {
                await Log.LogMessageAsync(user, "erro catch ao editar comando", "validation");
                await Bot.SndMsg(user, Resources.FormatInvalid);
            }
        }

        #endregion

        #region ListTask
        
        else if (Commands.ListCommand.Split(",").Select(c => c.Trim()).Contains(command))
            await TodoData.GetAndNotifyPendingTasksAsync(user);

        #endregion

        #region DeleteTask

        else if (Commands.DeleteCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            var title = message.Text.Substring(message.Text.IndexOf(' ') + 1).Trim();

            if (await TodoData.TaskExistsAsync(title, user) is false)
            {
                await Log.LogMessageAsync(user, $"this tasks exists | title: {title}", "task_action");
                await Bot.SndMsg(user, Resources.TaskNotFound);
                return;
            }

            await TodoData.RemoveTaskAsync(title, user);

            await Bot.SndMsg(user, $"Tarefa '{title}' removida com sucesso!");

            await Log.LogMessageAsync(user, $"task removed with success | title: {title}", "task_action");
        }

        #endregion

        #region ShowCreatores

        else if (command == "/creditos")
        {
            await Bot.SndMsg(
                user,
                "WhatsTodo - Desenvolvido pela equipe TC\n"
                    + "Versão 1.0.0 Alpha\n\n"
                    + "Github: https://github.com/MilyZani"
            );
        }

        #endregion

        #region ShowHelp

        else if (Commands.HelpCommand.Split(",").Select(c => c.Trim()).Contains(command))
        {
            await Bot.SndMsg(user, Resources.HelpMessageText);
        }

        #endregion

        #region TaskElse

        else
        {
            await Bot.SndMsg(user, Resources.HelpMessageText);
        }

        #endregion

    }
}
