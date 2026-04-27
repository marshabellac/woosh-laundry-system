using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using woOshLaundrySystem.Data;
using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Interfaces;
using woOshLaundrySystem.Models;
using woOshLaundrySystem.Services;

namespace woOshLaundrySystem.Views;

public partial class CreateOrderPage : Page
{
    private readonly DashboardWindow _w;
    private readonly int _customerId;
    private readonly OrderService _service = new();
    private readonly TariffCatalog _catalog = new();
    private readonly PackageRepository _packageRepository = new();
    private readonly ObservableCollection<OrderDetail> _details = new();
    private bool _isUpdatingMode = false;

    public CreateOrderPage(DashboardWindow w, int id)
    {
        InitializeComponent();
        _w = w;
        _customerId = id;

        CustomerText.Text = $"Pelanggan ID: {id}";

        CategoryBox.ItemsSource = Enum.GetValues(typeof(LaundryItemCategory));
        SpeedBox.ItemsSource = Enum.GetValues(typeof(ServiceSpeed));
        ModeBox.ItemsSource = Enum.GetValues(typeof(ProcessingMode));

        CategoryBox.SelectedIndex = 0;
        ModeBox.SelectedItem = ProcessingMode.Regular;
        UpdateServiceOptions();
        UpdateSpeedOptions();
        UpdateInputVisibility();
        UpdateAutomaticMode();

        var emp = new EmployeeRepository().GetAll();
        ReceiverBox.ItemsSource = emp.Where(x => x.Role == "Admin").ToList();
        WorkerBox.ItemsSource = emp.Where(x => x.Role == "Worker").ToList();
        ReceiverBox.SelectedIndex = 0;
        WorkerBox.SelectedIndex = 0;

        ItemGrid.ItemsSource = _details;
        RecalculateSummary();
    }

    private void CategoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateServiceOptions();
        UpdateSpeedOptions();
        UpdateInputVisibility();
        UpdateChargeWeightText();
        UpdateAutomaticMode();
        RecalculateSummary();
    }

    private void ServiceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateSpeedOptions();
        UpdateAutomaticMode();
        RecalculateSummary();
    }

    private void SpeedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateAutomaticMode();
        RecalculateSummary();
    }

    private void ModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingMode) return;
        RecalculateSummary();
    }

    private void WeightBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateChargeWeightText();
        RecalculateSummary();
    }

    private void UpdateInputVisibility()
    {
        if (CategoryBox?.SelectedItem == null || WeightBox == null || QtyBox == null) return;

        var category = (LaundryItemCategory)CategoryBox.SelectedItem;
        bool isKilogram = category == LaundryItemCategory.Clothing;

        WeightLabel.Visibility = isKilogram ? Visibility.Visible : Visibility.Collapsed;
        WeightBox.Visibility = isKilogram ? Visibility.Visible : Visibility.Collapsed;
        if (ChargeWeightText != null)
            ChargeWeightText.Visibility = isKilogram ? Visibility.Visible : Visibility.Collapsed;

        QtyLabel.Visibility = isKilogram ? Visibility.Collapsed : Visibility.Visible;
        QtyBox.Visibility = isKilogram ? Visibility.Collapsed : Visibility.Visible;

        // Supaya input yang tidak dipakai tidak membingungkan saat presentasi.
        if (isKilogram)
        {
            QtyBox.Text = "1";
        }
        else
        {
            WeightBox.Text = "0";
        }
    }


    private void UpdateChargeWeightText()
    {
        if (ChargeWeightText == null || WeightBox == null || CategoryBox?.SelectedItem == null) return;

        var category = (LaundryItemCategory)CategoryBox.SelectedItem;
        if (category != LaundryItemCategory.Clothing)
        {
            ChargeWeightText.Text = string.Empty;
            return;
        }

        double actualWeight = double.TryParse(WeightBox.Text, out var w) ? w : 0;
        double chargedWeight = CalculateChargedWeight(actualWeight);
        ChargeWeightText.Text = $"Berat tagihan regular: {chargedWeight:0.##} kg. Berat aktual tetap disimpan: {actualWeight:0.##} kg.";
    }

    private double CalculateChargedWeight(double actualWeight)
    {
        if (actualWeight <= 0) return 0;
        double chargedWeight = Math.Ceiling(actualWeight);
        return chargedWeight < 3 ? 3 : chargedWeight;
    }
    private void UpdateServiceOptions()
    {
        if (ServiceBox == null || CategoryBox.SelectedItem == null) return;

        var category = (LaundryItemCategory)CategoryBox.SelectedItem;

        // Boneka dan karpet hanya boleh cuci saja sesuai business rules.
        if (category == LaundryItemCategory.Doll || category == LaundryItemCategory.Carpet)
        {
            ServiceBox.ItemsSource = new List<ServiceType> { ServiceType.WashOnly };
        }
        else
        {
            ServiceBox.ItemsSource = Enum.GetValues(typeof(ServiceType));
        }

        ServiceBox.SelectedIndex = 0;
    }

    private void UpdateSpeedOptions()
    {
        if (SpeedBox == null || CategoryBox.SelectedItem == null) return;

        var category = (LaundryItemCategory)CategoryBox.SelectedItem;

        // Karpet tidak mendukung express.
        if (category == LaundryItemCategory.Carpet)
        {
            SpeedBox.ItemsSource = new List<ServiceSpeed> { ServiceSpeed.Regular };
        }
        else
        {
            SpeedBox.ItemsSource = Enum.GetValues(typeof(ServiceSpeed));
        }

        SpeedBox.SelectedIndex = 0;
    }

    private void UpdateAutomaticMode()
    {
        if (ModeBox == null || ServiceBox?.SelectedItem == null || SpeedBox?.SelectedItem == null) return;

        var activePackage = _packageRepository.GetActivePackage(_customerId);
        var service = (ServiceType)ServiceBox.SelectedItem;
        var speed = (ServiceSpeed)SpeedBox.SelectedItem;
        ProcessingMode mode = ProcessingMode.Regular;

        // Express selalu regular dan tidak boleh memakai paket.
        if (activePackage != null && speed == ServiceSpeed.Regular && service == ServiceType.WashAndIron)
        {
            if (_details.Count == 0)
            {
                // Sebelum item ditambahkan, kalau pelanggan punya paket aktif, tampilkan Paket sebagai default.
                mode = ProcessingMode.PackageOnly;
            }
            else
            {
                int eligibleCount = 0;
                foreach (var d in _details)
                {
                    d.MarkPackageEligible(_catalog, service, speed);
                    if (d.IsPackageEligible)
                    {
                        eligibleCount++;
                    }
                }

                if (eligibleCount == _details.Count)
                    mode = ProcessingMode.PackageOnly;
                else if (eligibleCount > 0)
                    mode = ProcessingMode.Mixed;
                else
                    mode = ProcessingMode.Regular;
            }
        }

        _isUpdatingMode = true;
        ModeBox.SelectedItem = mode;
        _isUpdatingMode = false;
    }

    private void AddItem_Click(object s, RoutedEventArgs e)
    {
        try
        {
            var cat = (LaundryItemCategory)CategoryBox.SelectedItem;
            UpdateChargeWeightText();
            double weight = double.TryParse(WeightBox.Text, out var w) ? w : 0;
            int qty = int.TryParse(QtyBox.Text, out var q) ? q : 0;

            if (cat == LaundryItemCategory.Clothing && weight <= 0)
                throw new Exception("Berat pakaian harus lebih dari 0 kg.");

            if (cat != LaundryItemCategory.Clothing && qty <= 0)
                throw new Exception("Jumlah item harus lebih dari 0.");

            var d = _service.CreateDetail(cat, weight, qty, "");

            var selectedService = (ServiceType)ServiceBox.SelectedItem;
            var selectedSpeed = (ServiceSpeed)SpeedBox.SelectedItem;

            if (!d.Item!.SupportsService(selectedService))
                throw new Exception("Item tidak mendukung layanan yang dipilih.");

            if (selectedSpeed == ServiceSpeed.Express && !d.Item.SupportsExpress(selectedService))
                throw new Exception("Item tidak mendukung layanan express.");

            _details.Add(d);
            UpdateAutomaticMode();
            RecalculateSummary();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void RemoveItem_Click(object s, RoutedEventArgs e)
    {
        if (ItemGrid.SelectedItem is OrderDetail d)
            _details.Remove(d);

        UpdateAutomaticMode();
        RecalculateSummary();
    }

    private void RecalculateSummary()
    {
        if (SummaryText == null || ModeBox?.SelectedItem == null || ServiceBox?.SelectedItem == null || SpeedBox?.SelectedItem == null)
            return;

        UpdateAutomaticMode();

        if (_details.Count == 0)
        {
            var selectedMode = ModeBox.SelectedItem is ProcessingMode m ? m : ProcessingMode.Regular;
            SummaryText.Text = $"Mode: {selectedMode}\nPaket: 0 kg\nTotal Bayar Sekarang: Rp0";
            return;
        }

        try
        {
            var service = (ServiceType)ServiceBox.SelectedItem;
            var speed = (ServiceSpeed)SpeedBox.SelectedItem;
            var mode = (ProcessingMode)ModeBox.SelectedItem;

            // Business rule: order express selalu regular dan tidak memakai paket.
            if (speed == ServiceSpeed.Express)
                mode = ProcessingMode.Regular;

            int total = 0;
            double packageWeight = 0;

            foreach (var d in _details)
            {
                var rule = _catalog.GetRule(d.ItemCategory, service, speed);
                if (rule == null)
                    throw new Exception("Item tidak mendukung layanan yang dipilih.");

                d.MarkPackageEligible(_catalog, service, speed);
                IPriceCalculator calculator = d.UnitType == UnitType.Kilogram
                    ? new MeasurementPriceCalculator()
                    : new CountedItemPriceCalculator();

                if (mode == ProcessingMode.PackageOnly)
                {
                    if (!d.IsPackageEligible)
                        throw new Exception("Paket hanya berlaku untuk pakaian kiloan, cuci + setrika, regular.");

                    d.CoverByPackage();
                    d.Notes = "Ditanggung paket. Kuota dipotong berdasarkan berat aktual.";
                    packageWeight += d.ActualWeight();
                }
                else if (mode == ProcessingMode.Mixed && d.IsPackageEligible)
                {
                    d.CoverByPackage();
                    d.Notes = "Ditanggung paket. Kuota dipotong berdasarkan berat aktual.";
                    packageWeight += d.ActualWeight();
                }
                else
                {
                    d.ChargeAsRegular(calculator.CalculateRegularCharge(d, rule));
                    if (d.UnitType == UnitType.Kilogram)
                    {
                        double actual = d.WeightKg ?? 0;
                        double charged = CalculateChargedWeight(actual);
                        d.Notes = $"Berat aktual {actual:0.##} kg, ditagihkan {charged:0.##} kg.";
                    }
                    else if (string.IsNullOrWhiteSpace(d.Notes))
                    {
                        d.Notes = "Dihitung berdasarkan jumlah item.";
                    }
                    total += d.RegularSubtotal;
                }
            }

            if (packageWeight > 0)
            {
                var activePackage = _packageRepository.GetActivePackage(_customerId);
                if (activePackage == null)
                    throw new Exception("Pelanggan tidak memiliki paket aktif.");

                if (packageWeight > activePackage.RemainingQuotaKg)
                    throw new Exception("Kuota paket tidak cukup.");
            }

            ItemGrid.Items.Refresh();
            SummaryText.Text =
                $"Mode: {mode}\n" +
                $"Paket Digunakan: {packageWeight:0.##} kg\n" +
                $"Total Bayar Sekarang: {FormatRupiah(total)}";
        }
        catch (Exception ex)
        {
            ItemGrid.Items.Refresh();
            SummaryText.Text = $"Ringkasan belum valid: {ex.Message}";
        }
    }

    private string FormatRupiah(int amount)
    {
        return "Rp" + amount.ToString("N0").Replace(",", ".");
    }

    private void Create_Click(object s, RoutedEventArgs e)
    {
        try
        {
            UpdateAutomaticMode();

            int id = _service.CreateOrder(
                _customerId,
                _details.ToList(),
                (ServiceType)ServiceBox.SelectedItem,
                (ServiceSpeed)SpeedBox.SelectedItem,
                (ProcessingMode)ModeBox.SelectedItem,
                (int)ReceiverBox.SelectedValue,
                (int)WorkerBox.SelectedValue,
                NotesBox.Text);

            MessageBox.Show("Order berhasil dibuat dan dibayar.");
            _w.Navigate(new OrderDetailPage(_w, id));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
