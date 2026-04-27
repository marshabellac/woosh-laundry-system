using System.Windows;
using System.Windows.Controls;
namespace woOshLaundrySystem.Views; public partial class PickedUpSuccessPage:Page{private readonly DashboardWindow _w;public PickedUpSuccessPage(DashboardWindow w){InitializeComponent();_w=w;}private void Back_Click(object s,RoutedEventArgs e)=>_w.Navigate(new DashboardPage(_w));}
