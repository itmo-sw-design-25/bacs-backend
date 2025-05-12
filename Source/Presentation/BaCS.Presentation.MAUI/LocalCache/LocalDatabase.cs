namespace BaCS.Presentation.MAUI.LocalCache;

using Services;
using SQLite;

public class LocalDatabase : IDisposable
{
    SQLiteAsyncConnection? database;
    IAuthentificationService service;

    public LocalDatabase(IAuthentificationService service)
    {
        this.service = service;
        Subscribe(true);


    }

    public void Init()
    {
        if(database != null) return;

        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    }
    private async Task ClearAsync()
    {
        if (File.Exists(Constants.DatabasePath))
        {
            await database.CloseAsync();
            database = null;
            File.Delete(Constants.DatabasePath);
        }

        Init();
    }

    private void Subscribe(bool s)
    {
        if (s)
        {
            service.OnReloginRequested += ClearAsync;
        }
        else
        {
            service.OnReloginRequested -= ClearAsync;
        }
    }

    public void Dispose()
    {
        Subscribe(false);
    }
}
