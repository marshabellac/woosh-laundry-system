using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public LaundryItem? Item { get; set; }
    public LaundryItemCategory ItemCategory { get; set; }
    public string ItemName { get; set; } = "";
    public UnitType UnitType { get; set; }
    public double? WeightKg { get; set; }
    public int? Quantity { get; set; }
    public bool IsPackageEligible { get; set; }
    public bool IsCoveredByPackage { get; set; }
    public int RegularSubtotal { get; set; }
    public string Notes { get; set; } = "";
    public double ActualWeight() => WeightKg ?? 0;
    public int QuantityValue() => Quantity ?? 0;
    public void MarkPackageEligible(TariffCatalog catalog, ServiceType serviceType, ServiceSpeed speed)
    {
        IsPackageEligible = ItemCategory == LaundryItemCategory.Clothing && UnitType == UnitType.Kilogram && serviceType == ServiceType.WashAndIron && speed == ServiceSpeed.Regular;
    }
    public void CoverByPackage() { IsCoveredByPackage = true; RegularSubtotal = 0; }
    public void ChargeAsRegular(int amount) { IsCoveredByPackage = false; RegularSubtotal = amount; }
}
