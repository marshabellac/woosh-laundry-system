using System.Windows;
using System.Windows.Controls;
namespace woOshLaundrySystem.Views; public partial class PackageSuccessPage:Page{private readonly DashboardWindow _w;private readonly int _id;public PackageSuccessPage(DashboardWindow w,int id){InitializeComponent();_w=w;_id=id;}private void Back_Click(object s,RoutedEventArgs e)=>_w.Navigate(new CustomerProfilePage(_w,_id));}
