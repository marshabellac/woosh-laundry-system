using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class CustomerPackage
{
    public int CustomerPackageId { get; set; }
    public Customer? Customer { get; set; }
    public int CustomerId { get; set; }
    public LaundryPackagePlan? Plan { get; set; }
    public int PackagePlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialQuotaKg { get; set; }
    public double RemainingQuotaKg { get; set; }
    public PackageStatus Status { get; set; } = PackageStatus.Active;
    public bool IsActive(DateTime today) => Status == PackageStatus.Active && today.Date <= EndDate.Date && RemainingQuotaKg > 0;
    public bool CanBeUsedFor(OrderDetail detail, ServiceType serviceType, ServiceSpeed speed) => Plan != null && detail.Item != null && Plan.CanCover(detail.Item, serviceType, speed);
    public void Consume(double weightKg)
    {
        if (weightKg > RemainingQuotaKg) throw new InvalidOperationException("Kuota paket tidak cukup.");
        RemainingQuotaKg -= weightKg;
        if (RemainingQuotaKg <= 0) Status = PackageStatus.Expired;
    }
}
