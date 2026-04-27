using woOshLaundrySystem.Data;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class CustomerService
{
    private readonly CustomerRepository _customers=new(); private readonly PackageRepository _packages=new();
    public int AddCustomer(string name,string phone,string address,string note){ if(string.IsNullOrWhiteSpace(name)) throw new Exception("Nama pelanggan wajib diisi."); if(string.IsNullOrWhiteSpace(phone)) throw new Exception("Nomor HP wajib diisi."); return _customers.Add(new Customer{Name=name,Phone=phone,Address=address,ImportantNote=note}); }
    public List<Customer> SearchCustomer(string q)=>_customers.Search(q);
    public Customer? GetCustomerProfile(int id)=>_customers.GetById(id);
    public CustomerPackage? CheckActivePackage(int customerId)=>_packages.GetActivePackage(customerId);
}
