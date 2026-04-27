using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class TariffRule
{
    public int TariffRuleId { get; set; }
    public LaundryItemCategory ItemCategory { get; set; }
    public ServiceType ServiceType { get; set; }
    public ServiceSpeed ServiceSpeed { get; set; }
    public UnitType UnitType { get; set; }
    public int Price { get; set; }
    public int EstimatedHours { get; set; }
    public bool SupportsExpress { get; set; }
}