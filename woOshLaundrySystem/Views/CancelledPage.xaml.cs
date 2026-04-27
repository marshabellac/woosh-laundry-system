using System.Windows;
using System.Windows.Controls;
namespace woOshLaundrySystem.Views; public partial class CancelledPage:Page{private readonly DashboardWindow _w;public CancelledPage(DashboardWindow w){InitializeComponent();_w=w;}private void Back_Click(object s,RoutedEventArgs e)=>_w.Navigate(new AllOrdersPage(_w));}
