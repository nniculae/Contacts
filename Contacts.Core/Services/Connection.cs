using Microsoft.Data.Sqlite;

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

        return new SqliteConnection(connectionStringBuilder);
    }
}
