using Microsoft.Data.Sqlite;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Data;
public class EmployeeRepository
{
    public List<Employee> GetAll(){var list=new List<Employee>();using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="SELECT * FROM Employees ORDER BY Role,Name";using var r=cmd.ExecuteReader();while(r.Read())list.Add(Map(r));return list;}
    public Employee? GetById(int id){using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="SELECT * FROM Employees WHERE EmployeeId=@id";cmd.Parameters.AddWithValue("@id",id);using var r=cmd.ExecuteReader();return r.Read()?Map(r):null;}
    Employee Map(SqliteDataReader r){var role=r.GetString(2);Employee e=role=="Admin"?new AdminEmployee():new LaundryWorker();e.EmployeeId=r.GetInt32(0);e.Name=r.GetString(1);return e;}
}
