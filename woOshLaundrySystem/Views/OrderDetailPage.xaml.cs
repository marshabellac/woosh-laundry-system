using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Services;
using woOshLaundrySystem.Utilities;

namespace woOshLaundrySystem.Views;

public partial class OrderDetailPage : Page
{
    private readonly DashboardWindow _w;
    private readonly int _id;
    private readonly OrderService _service = new();

    public OrderDetailPage(DashboardWindow w, int id)
    {
        InitializeComponent();
        _w = w;
        _id = id;
        Load();
    }

    private void Load()
    {
        var o = _service.GetOrderDetail(_id);
        if (o == null) return;

        OrderText.Text = o.OrderNumber;
        CustomerText.Text = $"Pelanggan ID: {o.CustomerId}";
        InfoText.Text = $"Status: {o.OrderStatus} | Mode: {o.ProcessingMode} | Layanan: {o.ServiceType} | Kecepatan: {o.ServiceSpeed} | Estimasi selesai: {o.EstimatedFinishAt:dd/MM/yyyy HH:mm}";
        PaymentText.Text = $"{o.PaymentStatus} | Total Bayar: {CurrencyFormatter.Rupiah(o.GetAmountDue())}";
        TeamText.Text = $"Penerima: {o.Assignment.ReceiverEmployeeId} | Pekerja: {o.Assignment.ResponsibleEmployeeId}";
        DetailGrid.ItemsSource = o.Details;
    }

    private void Start_Click(object s, RoutedEventArgs e)
    {
        Act(() => _service.StartProcessing(_id), null);
    }

    private void Finish_Click(object s, RoutedEventArgs e)
    {
        Act(() => _service.FinishOrder(_id), null);
    }

    private void Pickup_Click(object s, RoutedEventArgs e)
    {
        Act(() => _service.PickUpOrder(_id), new PickedUpSuccessPage(_w));
    }

    private void Cancel_Click(object s, RoutedEventArgs e)
    {
        Act(() => _service.CancelOrder(_id), new CancelledPage(_w));
    }

    private void Act(Action action, Page? nextPage)
    {
        try
        {
            action();
            MessageBox.Show("Status berhasil diperbarui.");
            if (nextPage == null) Load();
            else _w.Navigate(nextPage);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
