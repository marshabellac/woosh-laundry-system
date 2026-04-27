using woOshLaundrySystem.Data;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class OperationalCostService{ private readonly OperationalCostRepository _repo=new(); public MonthlyOperationalCostSummary GetThisMonth()=>_repo.GetMonth(DateTime.Now.Month,DateTime.Now.Year); }
