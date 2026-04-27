namespace woOshLaundrySystem.Models;
public class PackageUsageAllocation
{
    public double CoveredWeight { get; set; }
    public List<OrderDetail> CoveredDetails { get; set; } = new();
}
