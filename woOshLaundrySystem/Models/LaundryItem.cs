using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Interfaces;
namespace woOshLaundrySystem.Models;
public class LaundryItem : IServiceSupport
{
    public string ItemName { get; protected set; } = "";
    public LaundryItemCategory Category { get; protected set; }
    public UnitType UnitType { get; protected set; }
    public virtual bool SupportsService(ServiceType serviceType) => serviceType == ServiceType.WashOnly;
    public virtual bool SupportsExpress(ServiceType serviceType) => SupportsService(serviceType);
}
public class MeasuredLaundryItem : LaundryItem
{
    public MeasuredLaundryItem() { UnitType = UnitType.Kilogram; }
}
public class CountedLaundryItem : LaundryItem
{
    public CountedLaundryItem() { UnitType = UnitType.Item; }
}
public class ClothingItem : MeasuredLaundryItem
{
    public ClothingItem() { ItemName = "Pakaian"; Category = LaundryItemCategory.Clothing; }
    public override bool SupportsService(ServiceType serviceType) => true;
    public override bool SupportsExpress(ServiceType serviceType) => true;
}
public class BlanketItem : CountedLaundryItem
{
    public BlanketItem() { ItemName = "Selimut"; Category = LaundryItemCategory.Blanket; }
    public override bool SupportsService(ServiceType serviceType) => true;
    public override bool SupportsExpress(ServiceType serviceType) => true;
}
public class BedCoverItem : CountedLaundryItem
{
    public BedCoverItem() { ItemName = "Bed Cover"; Category = LaundryItemCategory.BedCover; }
    public override bool SupportsService(ServiceType serviceType) => true;
    public override bool SupportsExpress(ServiceType serviceType) => true;
}
public class CarpetItem : CountedLaundryItem
{
    public CarpetItem() { ItemName = "Karpet"; Category = LaundryItemCategory.Carpet; }
    public override bool SupportsService(ServiceType serviceType) => serviceType == ServiceType.WashOnly;
    public override bool SupportsExpress(ServiceType serviceType) => false;
}
public class DollItem : CountedLaundryItem
{
    public DollItem() { ItemName = "Boneka"; Category = LaundryItemCategory.Doll; }
    public override bool SupportsService(ServiceType serviceType) => serviceType == ServiceType.WashOnly;
    public override bool SupportsExpress(ServiceType serviceType) => serviceType == ServiceType.WashOnly;
}
