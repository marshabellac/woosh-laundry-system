using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Models;
using woOshLaundrySystem.Services;
namespace woOshLaundrySystem.Views;
public partial class CustomerListPage : Page
{
 private readonly DashboardWindow _window; private readonly CustomerService _service=new();
 public CustomerListPage(DashboardWindow w){InitializeComponent();_window=w;Load();}
 void Load(){CustomerGrid.ItemsSource=_service.SearchCustomer(SearchBox.Text);}
 private void SearchBox_TextChanged(object s,TextChangedEventArgs e)=>Load();
 private void Add_Click(object s,RoutedEventArgs e)=>_window.Navigate(new AddCustomerPage(_window));
 private void CustomerGrid_MouseDoubleClick(object s,System.Windows.Input.MouseButtonEventArgs e){if(CustomerGrid.SelectedItem is Customer c)_window.Navigate(new CustomerProfilePage(_window,c.CustomerId));}
}
