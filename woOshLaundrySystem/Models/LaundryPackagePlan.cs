using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class LaundryPackagePlan
{
    public int PackagePlanId { get; set; }
    public string PackageName { get; protected set; } = "";
    public int DurationDays { get; protected set; }
    public double InitialQuotaKg { get; protected set; }
    public int Price { get; protected set; }
    public LaundryPackagePlan() { }
    public LaundryPackagePlan(int id, string name, int days, double quota, int price) { PackagePlanId=id; PackageName=name; DurationDays=days; InitialQuotaKg=quota; Price=price; }
    public virtual bool CanCover(LaundryItem item, ServiceType serviceType, ServiceSpeed speed) => item.Category == LaundryItemCategory.Clothing && item.UnitType == UnitType.Kilogram && serviceType == ServiceType.WashAndIron && speed == ServiceSpeed.Regular;
}
public class WeeklyPackagePlan : LaundryPackagePlan { public WeeklyPackagePlan() : base(1,"Weekly Package",7,10,45000) { } }
public class MonthlyPackagePlan : LaundryPackagePlan { public MonthlyPackagePlan() : base(2,"Monthly Package",30,40,155000) { } }
