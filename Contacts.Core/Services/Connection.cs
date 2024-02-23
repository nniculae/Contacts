using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace Contacts.Core.Services;



public static class Connection
{
    public static SqliteConnection GetSqliteConnection()
    {
        var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var databasePath = Path.Combine(local, "contacts.sqlite");
        var demoDatabasePath = AppDomain.CurrentDomain.BaseDirectory + @"\Assets\contacts.sqlite";

        if (!File.Exists(databasePath))
        {
            File.Copy(demoDatabasePath, databasePath);
        }

        var connectionStringBuilder = new SqliteConnectionStringBuilder()
        {
            DataSource = databasePath
        }
         .ToString();

        var connection = new SqliteConnection(connectionStringBuilder);
        
        connection.Disposed += Connection_Disposed;
        connection.StateChange += Connection_StateChange;

        return connection;
    }

    private static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
    {
       Debug.Write("Connection state: " +  e.CurrentState.ToString() + Environment.NewLine);
    }

    private static void Connection_Disposed(object? sender, EventArgs e)
    {
        Debug.WriteLine("SqliteConnection Disposed");
    }
}
