using SQLite;
namespace MauiToDoList
{
    public static class DBcsatlakozas
    {
        private const string DbName = "teendolista.db";
        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;
        public static string Utvonal
        {
            get{return Path.Combine(FileSystem.AppDataDirectory, DbName);}
        }

        public static SQLiteAsyncConnection CreateConnection()
        {
            return new SQLiteAsyncConnection(Utvonal, Flags);
        }
    }
}