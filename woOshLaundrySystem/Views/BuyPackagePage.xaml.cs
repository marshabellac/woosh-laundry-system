using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Data;
using woOshLaundrySystem.Models;
using woOshLaundrySystem.Services;
using woOshLaundrySystem.Utilities;
namespace woOshLaundrySystem.Views;
public partial class BuyPackagePage:Page
{private readonly DashboardWindow _w;private readonly int _customerId;private readonly PackageService _service=new();public BuyPackagePage(DashboardWindow w,int id){InitializeComponent();_w=w;_customerId=id;CustomerText.Text=$"Pelanggan ID: {id}";PlanBox.ItemsSource=_service.GetPackagePlans();EmployeeBox.ItemsSource=new EmployeeRepository().GetAll().Where(x=>x.Role=="Admin").ToList();PlanBox.SelectedIndex=0;EmployeeBox.SelectedIndex=0;}private void PlanBox_SelectionChanged(object s,SelectionChangedEventArgs e){if(PlanBox.SelectedItem is LaundryPackagePlan p)SummaryText.Text=$"{p.PackageName} - {p.InitialQuotaKg} kg - {CurrencyFormatter.Rupiah(p.Price)}";}private void Buy_Click(object s,RoutedEventArgs e){try{if(PlanBox.SelectedValue==null)throw new Exception("Pilih paket terlebih dahulu.");_service.BuyPackage(_customerId,(int)PlanBox.SelectedValue);MessageBox.Show("Paket berhasil dibeli.");_w.Navigate(new PackageSuccessPage(_w,_customerId));}catch(Exception ex){MessageBox.Show(ex.Message);}}}
