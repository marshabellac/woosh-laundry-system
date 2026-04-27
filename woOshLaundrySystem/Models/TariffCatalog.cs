using woOshLaundrySystem.Data;
using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class TariffCatalog
{
    private readonly TariffRepository _repo = new();
    public List<TariffRule> Rules { get; set; } = new();
    public TariffCatalog() { Rules = _repo.GetAll(); }
    public TariffRule? GetRule(LaundryItemCategory category, ServiceType serviceType, ServiceSpeed speed) => Rules.FirstOrDefault(r => r.ItemCategory == category && r.ServiceType == serviceType && r.ServiceSpeed == speed);
    public bool Supports(LaundryItemCategory category, ServiceType serviceType, ServiceSpeed speed) => GetRule(category, serviceType, speed) != null;
}
