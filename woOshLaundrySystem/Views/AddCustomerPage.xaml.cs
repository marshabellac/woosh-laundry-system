using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Services;
namespace woOshLaundrySystem.Views;
public partial class AddCustomerPage : Page
{ private readonly DashboardWindow _window; private readonly CustomerService _service=new(); public AddCustomerPage(DashboardWindow w){InitializeComponent();_window=w;} private void Save_Click(object s,RoutedEventArgs e){try{int id=_service.AddCustomer(NameBox.Text,PhoneBox.Text,AddressBox.Text,NoteBox.Text);MessageBox.Show("Pelanggan berhasil disimpan.");_window.Navigate(new CustomerProfilePage(_window,id));}catch(Exception ex){MessageBox.Show(ex.Message);}}}
