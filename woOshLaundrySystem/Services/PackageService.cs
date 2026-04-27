using woOshLaundrySystem.Data;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class PackageService
{
    private readonly PackageRepository _repo=new();
    public List<LaundryPackagePlan> GetPackagePlans()=>_repo.GetPlans();
    public int BuyPackage(int customerId,int planId)
    {
        _repo.ExpireOldPackages();
        if(_repo.GetActivePackage(customerId)!=null) throw new Exception("Pelanggan masih memiliki paket aktif.");
        var plan=_repo.GetPlan(planId)??throw new Exception("Paket tidak ditemukan.");
        int pkgId=_repo.CreateCustomerPackage(customerId,plan); _repo.AddPurchase(customerId,planId,pkgId,plan.Price); return pkgId;
    }
}
