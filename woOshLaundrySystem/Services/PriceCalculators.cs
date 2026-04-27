using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Interfaces;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class MeasurementPriceCalculator : IPriceCalculator
{
    public int CalculateRegularCharge(OrderDetail d,TariffRule r){ double charged=d.WeightKg??0; if(d.UnitType==UnitType.Kilogram){ charged=Math.Ceiling(charged); if(charged<3) charged=3; } return (int)charged*r.Price; }
    public DateTime EstimateFinish(DateTime receivedAt,TariffRule rule)=>receivedAt.AddHours(rule.EstimatedHours);
}
public class CountedItemPriceCalculator : IPriceCalculator
{
    public int CalculateRegularCharge(OrderDetail d,TariffRule r)=> (d.Quantity??0)*r.Price;
    public DateTime EstimateFinish(DateTime receivedAt,TariffRule rule)=>receivedAt.AddHours(rule.EstimatedHours);
}
