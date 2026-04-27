namespace woOshLaundrySystem.Models;
public class Employee { public int EmployeeId { get; set; } public string Name { get; set; } = ""; public string Role { get; protected set; } = "Employee"; }
public class AdminEmployee : Employee { public AdminEmployee() { Role = "Admin"; } }
public class LaundryWorker : Employee { public LaundryWorker() { Role = "Worker"; } }
