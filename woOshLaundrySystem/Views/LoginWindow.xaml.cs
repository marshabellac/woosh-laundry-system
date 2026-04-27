using System.Windows;
namespace woOshLaundrySystem.Views;
public partial class LoginWindow : Window
{
    public LoginWindow(){InitializeComponent();}
    private void Login_Click(object sender,RoutedEventArgs e)
    {
        string u=UsernameBox.Text.Trim(); string p=PasswordBox.Password.Trim();
        if(u=="admin"&&p=="admin"){new DashboardWindow(u).Show();Close();}
        else MessageBox.Show("Username atau password salah.");
    }
}
