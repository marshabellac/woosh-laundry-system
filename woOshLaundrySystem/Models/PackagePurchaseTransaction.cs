namespace woOshLaundrySystem.Models;
public class PackagePurchaseTransaction : BusinessTransaction
{
    public int CustomerId { get; set; }
    public int PackagePlanId { get; set; }
    public int CustomerPackageId { get; set; }
    public int Amount { get; set; }
    public override int GetAmountDue() => Amount;
}
