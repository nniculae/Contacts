using Microsoft.Data.Sqlite;

namespace Contacts.Core.Services;
public class Connection
{
    public static SqliteConnection GetSqliteConnection()
    {
        var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var db = Path.Combine(local, "contacts.sqlite");
        // var DB_PATH = @"C:\projects\winui_apps\Contacts\Contacts\Db\contacts.sqlite";
        var connectionStringBuilder = new SqliteConnectionStringBuilder()
        {
            DataSource = db
        }
         .ToString();


        return new SqliteConnection(connectionStringBuilder);
    }
}
