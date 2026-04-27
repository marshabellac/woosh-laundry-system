using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Interfaces;
public interface IServiceSupport
{
    bool SupportsService(ServiceType serviceType);
    bool SupportsExpress(ServiceType serviceType);
}
