using Microsoft.Data.Sqlite;
using System.IO;
namespace woOshLaundrySystem.Data;
public static class DatabaseHelper
{
    public static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "woosh.db");
    public static string ConnectionString => $"Data Source={DatabasePath}";
    public static SqliteConnection GetConnection() => new(ConnectionString);
    public static void InitializeDatabase()
    {
        using var c = GetConnection(); c.Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Customers (CustomerId INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Phone TEXT NOT NULL, Address TEXT, ImportantNote TEXT);
CREATE TABLE IF NOT EXISTS Employees (EmployeeId INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Role TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS PackagePlans (PackagePlanId INTEGER PRIMARY KEY AUTOINCREMENT, PackageName TEXT, DurationDays INTEGER, InitialQuotaKg REAL, Price INTEGER);
CREATE TABLE IF NOT EXISTS CustomerPackages (CustomerPackageId INTEGER PRIMARY KEY AUTOINCREMENT, CustomerId INTEGER, PackagePlanId INTEGER, StartDate TEXT, EndDate TEXT, InitialQuotaKg REAL, RemainingQuotaKg REAL, Status TEXT);
CREATE TABLE IF NOT EXISTS PackagePurchaseTransactions (TransactionId INTEGER PRIMARY KEY AUTOINCREMENT, CustomerId INTEGER, PackagePlanId INTEGER, CustomerPackageId INTEGER, CreatedAt TEXT, Amount INTEGER, PaymentStatus TEXT);
CREATE TABLE IF NOT EXISTS LaundryOrders (OrderId INTEGER PRIMARY KEY AUTOINCREMENT, OrderNumber TEXT, CustomerId INTEGER, ReceivedAt TEXT, EstimatedFinishAt TEXT, ProcessingMode TEXT, ServiceType TEXT, ServiceSpeed TEXT, OrderStatus TEXT, PaymentStatus TEXT, AppliedPackageId INTEGER NULL, ReceiverEmployeeId INTEGER, ResponsibleEmployeeId INTEGER, RegularSubtotal INTEGER, PackageCoveredWeight REAL, AmountDueNow INTEGER, Notes TEXT);
CREATE TABLE IF NOT EXISTS OrderDetails (OrderDetailId INTEGER PRIMARY KEY AUTOINCREMENT, OrderId INTEGER, ItemCategory TEXT, ItemName TEXT, UnitType TEXT, WeightKg REAL NULL, Quantity INTEGER NULL, IsPackageEligible INTEGER, IsCoveredByPackage INTEGER, RegularSubtotal INTEGER, Notes TEXT);
CREATE TABLE IF NOT EXISTS TariffRules (TariffRuleId INTEGER PRIMARY KEY AUTOINCREMENT, ItemCategory TEXT, ServiceType TEXT, ServiceSpeed TEXT, UnitType TEXT, Price INTEGER, EstimatedHours INTEGER, SupportsExpress INTEGER);
CREATE TABLE IF NOT EXISTS OperationalCosts (OperationalCostId INTEGER PRIMARY KEY AUTOINCREMENT, Category TEXT, PeriodMonth INTEGER, PeriodYear INTEGER, Amount INTEGER);";
        cmd.ExecuteNonQuery();
    }
}
