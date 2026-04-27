using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Services;
namespace woOshLaundrySystem.Views;
public partial class CustomerProfilePage : Page
{ private readonly DashboardWindow _window; private readonly int _customerId; private readonly CustomerService _service=new(); public CustomerProfilePage(DashboardWindow w,int id){InitializeComponent();_window=w;_customerId=id;Load();} void Load(){var c=_service.GetCustomerProfile(_customerId); if(c==null)return; NameText.Text=c.Name; PhoneText.Text=c.Phone; var p=_service.CheckActivePackage(_customerId); if(p==null){PackageText.Text="Tidak ada paket aktif.";QuotaText.Text="";EndText.Text="";} else {PackageText.Text=p.Plan?.PackageName??"Paket Aktif";QuotaText.Text=$"Sisa kuota: {p.RemainingQuotaKg:0.##} kg";EndText.Text=$"Berakhir: {p.EndDate:dd/MM/yyyy}";}} private void Order_Click(object s,RoutedEventArgs e)=>_window.Navigate(new CreateOrderPage(_window,_customerId)); private void Package_Click(object s,RoutedEventArgs e)=>_window.Navigate(new BuyPackagePage(_window,_customerId)); private void Orders_Click(object s,RoutedEventArgs e)=>_window.Navigate(new AllOrdersPage(_window)); }
