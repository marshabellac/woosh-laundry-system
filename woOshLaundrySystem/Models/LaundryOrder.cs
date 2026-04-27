using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class LaundryOrder : BusinessTransaction
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = "";
    public Customer? Customer { get; set; }
    public int CustomerId { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.Now;
    public DateTime EstimatedFinishAt { get; set; }
    public List<OrderDetail> Details { get; set; } = new();
    public ServiceType ServiceType { get; set; }
    public ServiceSpeed ServiceSpeed { get; set; }
    public ProcessingMode ProcessingMode { get; set; } = ProcessingMode.Regular;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Received;
    public int? AppliedPackageId { get; set; }
    public PricingBreakdown Pricing { get; set; } = new();
    public OrderAssignment Assignment { get; set; } = new();
    public string Notes { get; set; } = "";
    public void AddDetail(OrderDetail detail) => Details.Add(detail);
    public override int GetAmountDue() => Pricing.AmountDueNow;
    public void StartProcessing()
    {
        if (PaymentStatus != woOshLaundrySystem.Enums.PaymentStatus.Paid) throw new InvalidOperationException("Order belum lunas.");
        if (!Assignment.HasResponsibleWorker()) throw new InvalidOperationException("Pekerja laundry belum dipilih.");
        OrderStatus = OrderStatus.InProcess;
    }
    public void Finish() { if (OrderStatus != OrderStatus.InProcess) throw new InvalidOperationException("Order belum diproses."); OrderStatus = OrderStatus.Finished; }
    public void PickUp() { if (OrderStatus != OrderStatus.Finished && OrderStatus != OrderStatus.LatePickup) throw new InvalidOperationException("Order belum selesai."); if (PaymentStatus != woOshLaundrySystem.Enums.PaymentStatus.Paid) throw new InvalidOperationException("Order belum lunas."); OrderStatus = OrderStatus.PickedUp; }
    public void Cancel() { if (OrderStatus != OrderStatus.Received) throw new InvalidOperationException("Order hanya bisa dibatalkan sebelum diproses."); OrderStatus = OrderStatus.Cancelled; }
    public void MarkLatePickup(DateTime today) { if (OrderStatus == OrderStatus.Finished && today > EstimatedFinishAt.AddDays(30)) OrderStatus = OrderStatus.LatePickup; }
}
