using Microsoft.Data.Sqlite;
using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Data;
public class OperationalCostRepository
{
 public MonthlyOperationalCostSummary GetMonth(int month,int year){var s=new MonthlyOperationalCostSummary();using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="SELECT * FROM OperationalCosts WHERE PeriodMonth=@m AND PeriodYear=@y";cmd.Parameters.AddWithValue("@m",month);cmd.Parameters.AddWithValue("@y",year);using var r=cmd.ExecuteReader();while(r.Read())s.Entries.Add(new OperationalCostEntry{OperationalCostId=r.GetInt32(0),Category=Enum.Parse<OperationalCostCategory>(r.GetString(1)),PeriodMonth=r.GetInt32(2),PeriodYear=r.GetInt32(3),Amount=r.GetInt32(4)});return s;}
}
