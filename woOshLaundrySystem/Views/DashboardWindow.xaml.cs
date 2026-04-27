using System.Windows;
namespace woOshLaundrySystem.Views;
public partial class DashboardWindow : Window
{
    public DashboardWindow(string username){InitializeComponent();WelcomeText.Text=$"Selamat datang, {username}";Navigate(new DashboardPage(this));}
    public void Navigate(object page)=>MainFrame.Navigate(page);
    private void Dashboard_Click(object s,RoutedEventArgs e)=>Navigate(new DashboardPage(this));
    private void Customers_Click(object s,RoutedEventArgs e)=>Navigate(new CustomerListPage(this));
    private void AddCustomer_Click(object s,RoutedEventArgs e)=>Navigate(new AddCustomerPage(this));
    private void Orders_Click(object s,RoutedEventArgs e)=>Navigate(new AllOrdersPage(this));
    private void NewOrder_Click(object s,RoutedEventArgs e)=>Navigate(new CustomerListPage(this));
    private void Logout_Click(object s,RoutedEventArgs e){new LoginWindow().Show();Close();}
}
