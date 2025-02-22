using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace WhatsTodo.Database;

public static class Database
{
    private static string DatabaseLocal = "Data Source=whatstodo.db";
    public static bool CreateDb()
    {
        try
        {
            using SqliteConnection connection = new(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS todos (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title VARCHAR(100) NOT NULL,
                    description TEXT,
                    notification_date DATE NOT NULL,
                    notification_time TIME NOT NULL,
                    is_completed BOOLEAN DEFAULT FALSE,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    user_phone VARCHAR(20) NOT NULL
                );";
            command.ExecuteNonQuery();

            command.CommandText = "PRAGMA table_info(todos);";
            bool hasCompletedAt = false;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader.GetString(1);
                    if (columnName == "completed_at")
                    {
                        hasCompletedAt = true;
                        break;
                    }
                }
            }

            if (!hasCompletedAt)
            {
                command.CommandText = "ALTER TABLE todos ADD COLUMN completed_at TIMESTAMP NULL;";
                command.ExecuteNonQuery();
            }

            command.CommandText = @"
                CREATE INDEX IF NOT EXISTS idx_notification_datetime 
                ON todos(notification_date, notification_time);
                
                CREATE INDEX IF NOT EXISTS idx_user_phone
                ON todos(user_phone);";
            command.ExecuteNonQuery();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar/atualizar banco de dados: {ex.Message}");
            return false;
        }
    }

    public static bool AddTask(
        string title,
        string description,
        DateTime notificationDate,
        TimeSpan notificationTime,
        string userPhone
    )
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO todos (
                    title,
                    description,
                    notification_date,
                    notification_time,
                    user_phone
                ) VALUES (
                    @title,
                    @description,
                    @notification_date,
                    @notification_time,
                    @user_phone
                );";

            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@notification_date", notificationDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@notification_time", notificationTime.ToString(@"hh\:mm"));
            command.Parameters.AddWithValue("@user_phone", userPhone);

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar tarefa: {ex.Message}");
            return false;
        }
    }

    public static bool UpdateTask(
        string title,
        string newDescription,
        DateTime newNotificationDate,
        TimeSpan newNotificationTime,
        string userPhone
    )
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
            UPDATE todos 
            SET description = @description,
                notification_date = @notification_date,
                notification_time = @notification_time
            WHERE LOWER(title) = LOWER(@title)
            AND user_phone = @user_phone
            AND is_completed = FALSE;";

            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@description", newDescription);
            command.Parameters.AddWithValue("@notification_date", newNotificationDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@notification_time", newNotificationTime.ToString(@"hh\:mm"));
            command.Parameters.AddWithValue("@user_phone", userPhone);

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar tarefa: {ex.Message}");
            return false;
        }
    }

    public static bool TaskExists(string title, string userPhone)
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT title FROM todos 
                    WHERE LOWER(title) = LOWER(@title)
                    AND user_phone = @user_phone 
                    AND is_completed = FALSE;";
                command.Parameters.AddWithValue("@title", title.Trim());
                command.Parameters.AddWithValue("@user_phone", userPhone);

                var existingTitle = command.ExecuteScalar();
                if (existingTitle != null)
                {
                    Console.WriteLine($"Found exact match: {existingTitle}");
                    return true;
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT title FROM todos 
                    WHERE (LOWER(title) LIKE LOWER(@titleStart) 
                    OR LOWER(title) LIKE LOWER(@titleMiddle)
                    OR LOWER(title) LIKE LOWER(@titleEnd))
                    AND user_phone = @user_phone
                    AND is_completed = FALSE;";

                var searchTitle = title.Trim();
                command.Parameters.AddWithValue("@titleStart", $"{searchTitle}%");
                command.Parameters.AddWithValue("@titleMiddle", $"% {searchTitle} %");
                command.Parameters.AddWithValue("@titleEnd", $"% {searchTitle}");
                command.Parameters.AddWithValue("@user_phone", userPhone);

                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader.GetString(0)}");
                    }
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar existência da tarefa: {ex.Message}");
            return false;
        }
    }

    public static List<dynamic> GetUserTasks(string userPhone)
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);

            var todoList = new List<object>();

            connection.Open();

            using var command = connection.CreateCommand();
            
            command.CommandText = @"
                SELECT title, description, notification_date, notification_time 
                FROM todos 
                WHERE user_phone = @phone 
            ";
            command.Parameters.AddWithValue("@phone", $"{userPhone}");

            using var sqlReader = command.ExecuteReader();

            while (sqlReader.Read())
            {
                todoList.Add(new
                {
                    Title = sqlReader.GetString(0),
                    Description = sqlReader.GetString(1),
                    NotificationDate = sqlReader.GetString(2).ToString(),
                    NotificationTime = sqlReader.GetDouble(3).ToString()
                });
            }

            return todoList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar tarefas: {ex.Message}");
            return new List<object>();
        }
    }

    public static bool RemoveTask(string title, string userPhone)
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM todos 
                WHERE LOWER(title) = LOWER(@title)
                AND user_phone = @user_phone
                AND is_completed = FALSE;";

            command.Parameters.AddWithValue("@title", title.Trim());
            command.Parameters.AddWithValue("@user_phone", userPhone);

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao remover tarefa: {ex.Message}");
            return false;
        }
    }

    public static List<(string UserPhone, string Title, string Description)> GetPendingNotifications()
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            TimeZoneInfo brasiliaTimeZone;
            try
            {
                brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            catch
            {
                brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }

            var nowInBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT user_phone, title, description
                FROM todos
                WHERE notification_date = @current_date
                AND notification_time <= @current_time
                AND is_completed = FALSE;";

            command.Parameters.AddWithValue("@current_date", nowInBrasilia.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@current_time", nowInBrasilia.ToString("HH:mm"));

            var notifications = new List<(string UserPhone, string Title, string Description)>();
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                notifications.Add((
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2)
                ));
            }

            return notifications;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar notificações pendentes: {ex.Message}");
            return new List<(string, string, string)>();
        }
    }

    public static bool MarkTaskAsCompleted(string title, string userPhone)
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE todos
                SET is_completed = TRUE
                WHERE LOWER(title) = LOWER(@title)
                AND user_phone = @user_phone;";

            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@user_phone", userPhone);

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao marcar tarefa como completa: {ex.Message}");
            return false;
        }
    }

    public static List<(int Id, string UserPhone, string Title, string Description)> GetTasksDueAt(DateTime currentTime)
    {
        var tasks = new List<(int Id, string UserPhone, string Title, string Description)>();
        
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT id, user_phone, title, description
                FROM todos
                WHERE date(notification_date) = date(@date)
                AND time(notification_time) = time(@time)
                AND is_completed = 0;";

            command.Parameters.AddWithValue("@date", currentTime.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@time", currentTime.ToString("HH:mm"));

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add((
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)
                ));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar tarefas para notificação: {ex.Message}");
        }

        return tasks;
    }

    public static async Task MarkNotificationSent(int taskId, DateTime sentTime)
    {
        try
        {
            using var connection = new SqliteConnection(DatabaseLocal);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE todos 
                SET is_completed = 1,
                    completed_at = datetime(@completed_at)
                WHERE id = @id;";

            command.Parameters.AddWithValue("@id", taskId);
            command.Parameters.AddWithValue("@completed_at", sentTime.ToString("yyyy-MM-dd HH:mm:ss"));

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao marcar notificação como enviada: {ex.Message}");
        }
    }
}