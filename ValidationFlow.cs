namespace WhatsTodo;

public class ValidadeStatus
{
    public bool TimeError { get; set; }
    public bool SintaxeError { get; set; }
    public bool TaskResult { get; set; }
    public bool CatchError { get; set; }
}

public class ValidationFlow
{
    public static ValidadeStatus ValidateAndUpdateTask(TaskCommand taskCommand, string userPhone)
    {
        try
        {
            if (!TimeSpan.TryParse(taskCommand.Time, out TimeSpan notificationTime))
                return new ValidadeStatus { TimeError = true };

            var notificationDate = DateTime.Today;

            if (notificationTime < DateTime.Now.TimeOfDay)
                notificationDate = notificationDate.AddDays(1);

            if (
                string.IsNullOrEmpty(taskCommand.Title)
                || string.IsNullOrEmpty(taskCommand.Description)
            )
                return new ValidadeStatus { SintaxeError = true };

            bool taskResult = Database.Database.UpdateTask(
                taskCommand.Title,
                taskCommand.Description,
                notificationDate,
                notificationTime,
                userPhone
            );

            return new ValidadeStatus { TaskResult = taskResult };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao validar e atualizar tarefa: {ex.Message}");
            return new ValidadeStatus { CatchError = true };
        }
    }

    public static ValidadeStatus ValidateAndAddTask(TaskCommand taskCommand, string userPhone)
    {
        try
        {
            if (!TimeSpan.TryParse(taskCommand.Time, out TimeSpan notificationTime))
                return new ValidadeStatus { TimeError = true };

            if (
                string.IsNullOrEmpty(taskCommand.Title)
                || string.IsNullOrEmpty(taskCommand.Description)
            )
                return new ValidadeStatus { SintaxeError = true };

            var notificationDate = DateTime.Today;
            var currentTime = DateTime.Now.TimeOfDay;

            if (notificationTime < currentTime)
                return new ValidadeStatus { TimeError = true };

            bool taskResult = Database.Database.AddTask(
                taskCommand.Title,
                taskCommand.Description,
                notificationDate,
                notificationTime,
                userPhone
            );

            return new ValidadeStatus { TaskResult = taskResult };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao validar e adicionar tarefa: {ex.Message}");
            return new ValidadeStatus { CatchError = true };
        }
    }
}