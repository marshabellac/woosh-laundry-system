namespace woOshLaundrySystem.Models;
public class OrderAssignment
{
    public Employee? Receiver { get; set; }
    public Employee? ResponsibleWorker { get; set; }
    public int ReceiverEmployeeId { get; set; }
    public int ResponsibleEmployeeId { get; set; }
    public void SetReceiver(Employee employee) { Receiver = employee; ReceiverEmployeeId = employee.EmployeeId; }
    public void AddResponsibleWorker(Employee employee) { ResponsibleWorker = employee; ResponsibleEmployeeId = employee.EmployeeId; }
    public bool HasResponsibleWorker() => ResponsibleEmployeeId > 0;
}
