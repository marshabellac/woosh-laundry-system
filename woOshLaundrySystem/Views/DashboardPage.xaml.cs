using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Services;
using woOshLaundrySystem.Utilities;
namespace woOshLaundrySystem.Views;
public partial class DashboardPage : Page
{
    private readonly DashboardWindow _window; private readonly DashboardService _service=new();
    public DashboardPage(DashboardWindow w){InitializeComponent();_window=w;Load();}
    void Load(){TodayText.Text=_service.CountTodayOrders().ToString();ActiveText.Text=_service.CountActiveOrders().ToString();FinishedText.Text=_service.CountFinishedOrders().ToString();LateText.Text=_service.CountLatePickupOrders().ToString();IncomeText.Text=CurrencyFormatter.Rupiah(_service.CalculateTodayIncome());RecentGrid.ItemsSource=_service.GetRecentOrders();}
    private void Customers_Click(object s,RoutedEventArgs e)=>_window.Navigate(new CustomerListPage(_window));
    private void Orders_Click(object s,RoutedEventArgs e)=>_window.Navigate(new AllOrdersPage(_window));
    private void AddCustomer_Click(object s,RoutedEventArgs e)=>_window.Navigate(new AddCustomerPage(_window));
}
