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
        var withoutCommand = message.Substring(message.IndexOf(' ') + 1);
        var time = withoutCommand.Substring(withoutCommand.Length - 5);
        var withoutTime = withoutCommand.Substring(0, withoutCommand.Length - 5).Trim();
        var title = withoutTime.Split(' ')[0];
        var description = withoutTime.Substring(title.Length).Trim();
        var date = DateTime.Parse(time).ToUniversalTime();
        return (title, description, date);
    }

    #endregion

    public static async Task Handler(dynamic message)
    {
        string user = message.User;

        #region VerifyUser
        if (await UserData.UserExists(user) is false)
        {
            await UserData.AddUser(user);
            await Bot.SndMsg(user, Resources.FirstUserMessage);
            return;
        }
        #endregion

        string text = message.Text;
        string command = text.Split(' ')[0].ToLower();

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
                    return;
                }

                var currentHour = DateTime.UtcNow.ToUniversalTime().AddHours(-3);

                if (notificationDate < currentHour)
                {
                    await Bot.SndMsg(user, "horario no passado");
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
                return;
            }
            catch
            {
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
                    await Bot.SndMsg(user, "Horario no passado");
                    return;
                }

                if (await TodoData.TaskExistsAsync(title, user) is false)
                {
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

                return;
            }
            catch
            {
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
            var taskTitle = message.Text.Substring(message.Text.IndexOf(' ') + 1).Trim();

            if (await TodoData.TaskExistsAsync(taskTitle, user) is false)
            {
                await Bot.SndMsg(user, Resources.TaskNotFound);
                return;
            }

            await TodoData.RemoveTaskAsync(taskTitle, user);

            await Bot.SndMsg(user, $"Tarefa '{taskTitle}' removida com sucesso!");
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
