using Microsoft.Data.Sqlite;
using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Data;
public class TariffRepository
{
 public List<TariffRule> GetAll(){var list=new List<TariffRule>();using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="SELECT * FROM TariffRules";using var r=cmd.ExecuteReader();while(r.Read())list.Add(new TariffRule{TariffRuleId=r.GetInt32(0),ItemCategory=Enum.Parse<LaundryItemCategory>(r.GetString(1)),ServiceType=Enum.Parse<ServiceType>(r.GetString(2)),ServiceSpeed=Enum.Parse<ServiceSpeed>(r.GetString(3)),UnitType=Enum.Parse<UnitType>(r.GetString(4)),Price=r.GetInt32(5),EstimatedHours=r.GetInt32(6),SupportsExpress=r.GetInt32(7)==1});return list;}
}
