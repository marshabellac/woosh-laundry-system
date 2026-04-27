using woOshLaundrySystem.Data;
using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class DashboardService
{
    private readonly OrderRepository _repo=new();
    public int CountTodayOrders()=>_repo.GetAll().Count(o=>o.ReceivedAt.Date==DateTime.Today);
    public int CountActiveOrders()=>_repo.GetAll().Count(o=>o.OrderStatus==OrderStatus.Received||o.OrderStatus==OrderStatus.InProcess);
    public int CountFinishedOrders()=>_repo.GetAll().Count(o=>o.OrderStatus==OrderStatus.Finished);
    public int CountLatePickupOrders()=>_repo.GetAll().Count(o=>o.OrderStatus==OrderStatus.LatePickup);
    public int CalculateTodayIncome()=>_repo.GetAll().Where(o=>o.ReceivedAt.Date==DateTime.Today).Sum(o=>o.GetAmountDue());
    public List<LaundryOrder> GetRecentOrders()=>_repo.GetAll().Take(5).ToList();
}
