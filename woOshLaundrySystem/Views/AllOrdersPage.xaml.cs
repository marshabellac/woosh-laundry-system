using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Models;
using woOshLaundrySystem.Services;
namespace woOshLaundrySystem.Views;
public partial class AllOrdersPage:Page
{private readonly DashboardWindow _w;private readonly OrderService _service=new();private string _status="Semua";public AllOrdersPage(DashboardWindow w){InitializeComponent();_w=w;Load();}void Load(){var list=_service.GetAllOrders(_status);if(!string.IsNullOrWhiteSpace(SearchBox.Text))list=list.Where(o=>o.OrderNumber.Contains(SearchBox.Text,StringComparison.OrdinalIgnoreCase)||o.CustomerId.ToString()==SearchBox.Text).ToList();OrderGrid.ItemsSource=list;}private void Filter_Click(object s,RoutedEventArgs e){_status=(s as Button)?.Tag?.ToString()??"Semua";Load();}private void SearchBox_TextChanged(object s,TextChangedEventArgs e)=>Load();private void OrderGrid_MouseDoubleClick(object s,System.Windows.Input.MouseButtonEventArgs e){if(OrderGrid.SelectedItem is LaundryOrder o)_w.Navigate(new OrderDetailPage(_w,o.OrderId));}}
