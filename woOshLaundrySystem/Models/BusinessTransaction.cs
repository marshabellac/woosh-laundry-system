using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class BusinessTransaction
{
    public int TransactionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public PaymentStatus PaymentStatus { get; set; } = woOshLaundrySystem.Enums.PaymentStatus.Paid;
    public virtual void MarkPaid() => PaymentStatus = woOshLaundrySystem.Enums.PaymentStatus.Paid;
    public virtual int GetAmountDue() => 0;
}
