using System.Windows;
using woOshLaundrySystem.Data;
namespace woOshLaundrySystem;
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DatabaseHelper.InitializeDatabase();
        SeedData.SeedAll();
    }
}
