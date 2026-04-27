using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Interfaces;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class RegularProcessingPolicy : IProcessingPolicy
{
    public void Validate(LaundryOrder o,TariffCatalog c){ foreach(var d in o.Details){ if(!c.Supports(d.ItemCategory,o.ServiceType,o.ServiceSpeed)) throw new Exception("Item tidak mendukung layanan yang dipilih."); } }
    public PackageUsageAllocation AllocatePackageCoverage(LaundryOrder o)=>new();
}
public class PackageOnlyProcessingPolicy : IProcessingPolicy
{
    public void Validate(LaundryOrder o,TariffCatalog c){ if(o.ServiceSpeed==ServiceSpeed.Express) throw new Exception("Paket hanya berlaku untuk regular."); foreach(var d in o.Details){ d.MarkPackageEligible(c,o.ServiceType,o.ServiceSpeed); if(!d.IsPackageEligible) throw new Exception("Paket hanya berlaku untuk pakaian kiloan, cuci + setrika, regular."); } }
    public PackageUsageAllocation AllocatePackageCoverage(LaundryOrder o){var a=new PackageUsageAllocation(); foreach(var d in o.Details.Where(x=>x.IsPackageEligible)){a.CoveredDetails.Add(d);a.CoveredWeight+=d.ActualWeight();} return a;}
}
public class MixedProcessingPolicy : IProcessingPolicy
{
    public void Validate(LaundryOrder o,TariffCatalog c){ foreach(var d in o.Details){ if(!c.Supports(d.ItemCategory,o.ServiceType,o.ServiceSpeed)) throw new Exception("Item tidak mendukung layanan yang dipilih."); d.MarkPackageEligible(c,o.ServiceType,o.ServiceSpeed); } }
    public PackageUsageAllocation AllocatePackageCoverage(LaundryOrder o){var a=new PackageUsageAllocation(); foreach(var d in o.Details.Where(x=>x.IsPackageEligible)){a.CoveredDetails.Add(d);a.CoveredWeight+=d.ActualWeight();} return a;}
}
