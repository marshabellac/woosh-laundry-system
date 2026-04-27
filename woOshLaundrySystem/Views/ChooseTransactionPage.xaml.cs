using System.Windows;
using System.Windows.Controls;
namespace woOshLaundrySystem.Views;
public partial class ChooseTransactionPage:Page{private readonly DashboardWindow _w;private readonly int _id;public ChooseTransactionPage(DashboardWindow w,int customerId){InitializeComponent();_w=w;_id=customerId;}private void Order_Click(object s,RoutedEventArgs e)=>_w.Navigate(new CreateOrderPage(_w,_id));private void Package_Click(object s,RoutedEventArgs e)=>_w.Navigate(new BuyPackagePage(_w,_id));}
