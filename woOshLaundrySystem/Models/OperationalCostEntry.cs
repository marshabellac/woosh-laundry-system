using woOshLaundrySystem.Enums;
namespace woOshLaundrySystem.Models;
public class OperationalCostEntry
{
    public int OperationalCostId { get; set; }
    public OperationalCostCategory Category { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public int Amount { get; set; }
}
public class MonthlyOperationalCostSummary
{
    public List<OperationalCostEntry> Entries { get; set; } = new();
    public int GetTotal() => Entries.Sum(e => e.Amount);
}
