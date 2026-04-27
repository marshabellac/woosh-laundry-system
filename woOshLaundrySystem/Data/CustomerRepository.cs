using Microsoft.Data.Sqlite;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Data;
public class CustomerRepository
{
    public int Add(Customer x){using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="INSERT INTO Customers(Name,Phone,Address,ImportantNote) VALUES(@n,@p,@a,@i); SELECT last_insert_rowid();";cmd.Parameters.AddWithValue("@n",x.Name);cmd.Parameters.AddWithValue("@p",x.Phone);cmd.Parameters.AddWithValue("@a",x.Address);cmd.Parameters.AddWithValue("@i",x.ImportantNote);return Convert.ToInt32(cmd.ExecuteScalar());}
    public List<Customer> Search(string q=""){var list=new List<Customer>();using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="SELECT * FROM Customers WHERE Name LIKE @q OR Phone LIKE @q ORDER BY Name";cmd.Parameters.AddWithValue("@q","%"+q+"%");using var r=cmd.ExecuteReader();while(r.Read())list.Add(Map(r));return list;}
    public Customer? GetById(int id){using var c=DatabaseHelper.GetConnection();c.Open();using var cmd=c.CreateCommand();cmd.CommandText="SELECT * FROM Customers WHERE CustomerId=@id";cmd.Parameters.AddWithValue("@id",id);using var r=cmd.ExecuteReader();return r.Read()?Map(r):null;}
    Customer Map(SqliteDataReader r)=>new(){CustomerId=r.GetInt32(0),Name=r.GetString(1),Phone=r.GetString(2),Address=r.IsDBNull(3)?"":r.GetString(3),ImportantNote=r.IsDBNull(4)?"":r.GetString(4)};
}
