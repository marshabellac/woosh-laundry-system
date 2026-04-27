namespace woOshLaundrySystem.Models;
public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string ImportantNote { get; set; } = "";
    public bool HasActivePackage(DateTime onDate, CustomerPackage? package) => package != null && package.IsActive(onDate);
}
