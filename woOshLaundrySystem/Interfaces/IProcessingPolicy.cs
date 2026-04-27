using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Interfaces;
public interface IProcessingPolicy
{
    void Validate(LaundryOrder order, TariffCatalog tariffCatalog);
    PackageUsageAllocation AllocatePackageCoverage(LaundryOrder order);
}
