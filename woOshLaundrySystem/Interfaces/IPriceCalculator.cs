using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Interfaces;
public interface IPriceCalculator
{
    int CalculateRegularCharge(OrderDetail detail, TariffRule rule);
    DateTime EstimateFinish(DateTime receivedAt, TariffRule rule);
}