using Microsoft.Data.Sqlite;

namespace woOshLaundrySystem.Data;

public static class SeedData
{
    public static void SeedAll()
    {
        SeedEmployees();
        SeedPackagePlans();
        SeedTariffs();
        SeedOperationalCosts();
        SeedDummyData();
    }

    static int Count(string table)
    {
        using var c = DatabaseHelper.GetConnection();
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM {table}";
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    static void Exec(string sql, params object[] values)
    {
        using var c = DatabaseHelper.GetConnection();
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql;
        for (int i = 0; i < values.Length; i++)
            cmd.Parameters.AddWithValue("@p" + i, values[i] ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    static int InsertAndGetId(string sql, params object[] values)
    {
        using var c = DatabaseHelper.GetConnection();
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql + "; SELECT last_insert_rowid();";
        for (int i = 0; i < values.Length; i++)
            cmd.Parameters.AddWithValue("@p" + i, values[i] ?? DBNull.Value);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    static void SeedEmployees()
    {
        if (Count("Employees") > 0) return;
        Exec("INSERT INTO Employees(Name,Role) VALUES(@p0,@p1)", "Admin Budi", "Admin");
        Exec("INSERT INTO Employees(Name,Role) VALUES(@p0,@p1)", "Admin Sari", "Admin");
        Exec("INSERT INTO Employees(Name,Role) VALUES(@p0,@p1)", "Pekerja Dina", "Worker");
        Exec("INSERT INTO Employees(Name,Role) VALUES(@p0,@p1)", "Pekerja Rina", "Worker");
    }

    static void SeedPackagePlans()
    {
        if (Count("PackagePlans") > 0) return;
        Exec("INSERT INTO PackagePlans(PackageName,DurationDays,InitialQuotaKg,Price) VALUES(@p0,@p1,@p2,@p3)", "Weekly Package", 7, 10, 45000);
        Exec("INSERT INTO PackagePlans(PackageName,DurationDays,InitialQuotaKg,Price) VALUES(@p0,@p1,@p2,@p3)", "Monthly Package", 30, 40, 155000);
    }

    static void AddTariff(string cat, string service, string speed, string unit, int price, int hours, int supportsExpress)
    {
        Exec("INSERT INTO TariffRules(ItemCategory,ServiceType,ServiceSpeed,UnitType,Price,EstimatedHours,SupportsExpress) VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)", cat, service, speed, unit, price, hours, supportsExpress);
    }

    static void SeedTariffs()
    {
        if (Count("TariffRules") > 0) return;
        AddTariff("Clothing", "WashOnly", "Regular", "Kilogram", 5000, 24, 1);
        AddTariff("Clothing", "WashOnly", "Express", "Kilogram", 7000, 6, 1);
        AddTariff("Clothing", "IronOnly", "Regular", "Kilogram", 5000, 24, 1);
        AddTariff("Clothing", "IronOnly", "Express", "Kilogram", 7000, 4, 1);
        AddTariff("Clothing", "WashAndIron", "Regular", "Kilogram", 7000, 48, 1);
        AddTariff("Clothing", "WashAndIron", "Express", "Kilogram", 10000, 24, 1);
        AddTariff("Blanket", "WashOnly", "Regular", "Item", 18000, 48, 1);
        AddTariff("Blanket", "WashOnly", "Express", "Item", 26000, 24, 1);
        AddTariff("Blanket", "IronOnly", "Regular", "Item", 12000, 24, 1);
        AddTariff("Blanket", "IronOnly", "Express", "Item", 17000, 6, 1);
        AddTariff("Blanket", "WashAndIron", "Regular", "Item", 25000, 72, 1);
        AddTariff("Blanket", "WashAndIron", "Express", "Item", 35000, 48, 1);
        AddTariff("BedCover", "WashOnly", "Regular", "Item", 22000, 72, 1);
        AddTariff("BedCover", "WashOnly", "Express", "Item", 32000, 48, 1);
        AddTariff("BedCover", "IronOnly", "Regular", "Item", 15000, 24, 1);
        AddTariff("BedCover", "IronOnly", "Express", "Item", 20000, 6, 1);
        AddTariff("BedCover", "WashAndIron", "Regular", "Item", 30000, 96, 1);
        AddTariff("BedCover", "WashAndIron", "Express", "Item", 42000, 48, 1);
        AddTariff("Doll", "WashOnly", "Regular", "Item", 22000, 72, 1);
        AddTariff("Doll", "WashOnly", "Express", "Item", 30000, 48, 1);
        AddTariff("Carpet", "WashOnly", "Regular", "Item", 35000, 96, 0);
    }

    static void SeedOperationalCosts()
    {
        if (Count("OperationalCosts") > 0) return;
        int m = DateTime.Now.Month, y = DateTime.Now.Year;
        (string, int)[] data =
        {
            ("Electricity", 400000), ("Water", 150000), ("MachineMaintenance", 100000),
            ("Detergent", 130000), ("Fragrance", 100000), ("LaundryPlastic", 35000),
            ("ReceiptPaper", 35000), ("SmallEquipment", 20000), ("Other", 50000)
        };
        foreach (var d in data)
            Exec("INSERT INTO OperationalCosts(Category,PeriodMonth,PeriodYear,Amount) VALUES(@p0,@p1,@p2,@p3)", d.Item1, m, y, d.Item2);
    }

    // Dummy data is helpful for presentation/demo, but still simple enough for students to understand.
    // It runs only when there are no customers yet, so it will not duplicate data every time the app starts.
    static void SeedDummyData()
    {
        if (Count("Customers") > 0) return;

        int angelina = AddCustomer("Angelina Gracilda", "08123456789", "Lippo Village, Tangerang", "Suka pakaian dilipat rapi.");
        int cindi = AddCustomer("Cindi Kristiana Sianipar", "08129876543", "Karawaci", "Pelanggan paket mingguan.");
        int marshabella = AddCustomer("Marshabella Chrisfrans", "08234567890", "Gading Serpong", "Sering order express.");
        int nicole = AddCustomer("Nicole Gabriela Jewel Charles", "08345678901", "BSD", "Punya paket expired untuk contoh.");

        DateTime today = DateTime.Now.Date;

        int angelinaMonthly = AddCustomerPackage(angelina, 2, today.AddDays(-3), today.AddDays(27), 40, 34.5, "Active");
        AddPackagePurchase(angelina, 2, angelinaMonthly, today.AddDays(-3), 155000);

        int cindiWeekly = AddCustomerPackage(cindi, 1, today.AddDays(-1), today.AddDays(6), 10, 7.5, "Active");
        AddPackagePurchase(cindi, 1, cindiWeekly, today.AddDays(-1), 45000);

        int nicoleExpired = AddCustomerPackage(nicole, 1, today.AddDays(-12), today.AddDays(-5), 10, 0, "Expired");
        AddPackagePurchase(nicole, 1, nicoleExpired, today.AddDays(-12), 45000);

        int order1 = AddOrder("ORD-" + today.ToString("yyyyMMdd") + "-1001", angelina, today, today.AddDays(2), "PackageOnly", "WashAndIron", "Regular", "Received", "Paid", angelinaMonthly, 1, 3, 0, 2.5, 0, "Demo: order paket pakaian.");
        AddOrderDetail(order1, "Clothing", "Pakaian", "Kilogram", 2.5, null, 1, 1, 0, "Ditanggung paket.");

        int order2 = AddOrder("ORD-" + today.ToString("yyyyMMdd") + "-1002", cindi, today, today.AddDays(3), "Mixed", "WashAndIron", "Regular", "InProcess", "Paid", cindiWeekly, 2, 4, 25000, 3.0, 25000, "Demo: campuran paket dan regular.");
        AddOrderDetail(order2, "Clothing", "Pakaian", "Kilogram", 3.0, null, 1, 1, 0, "Bagian pakaian pakai paket.");
        AddOrderDetail(order2, "Blanket", "Selimut", "Item", null, 1, 0, 0, 25000, "Selimut dihitung regular.");

        int order3 = AddOrder("ORD-" + today.ToString("yyyyMMdd") + "-1003", marshabella, today, today.AddHours(6), "Regular", "WashOnly", "Express", "Finished", "Paid", null, 1, 3, 70000, 0, 70000, "Demo: express tidak memakai paket.");
        AddOrderDetail(order3, "Clothing", "Pakaian", "Kilogram", 6.2, null, 0, 0, 70000, "Berat dibulatkan menjadi 7 kg.");

        int order4 = AddOrder("ORD-" + today.ToString("yyyyMMdd") + "-1004", nicole, today.AddDays(-1), today.AddDays(3), "Regular", "WashOnly", "Regular", "Received", "Paid", null, 2, 4, 70000, 0, 70000, "Demo: karpet regular.");
        AddOrderDetail(order4, "Carpet", "Karpet", "Item", null, 2, 0, 0, 70000, "Karpet tidak mendukung express.");

        int order5 = AddOrder("ORD-" + today.AddDays(-1).ToString("yyyyMMdd") + "-1005", marshabella, today.AddDays(-1), today, "Regular", "WashAndIron", "Regular", "PickedUp", "Paid", null, 1, 3, 21000, 0, 21000, "Demo: order sudah diambil.");
        AddOrderDetail(order5, "Clothing", "Pakaian", "Kilogram", 2.1, null, 0, 0, 21000, "Minimum charge 3 kg.");

        int order6 = AddOrder("ORD-" + today.AddDays(-2).ToString("yyyyMMdd") + "-1006", nicole, today.AddDays(-2), today.AddDays(2), "Regular", "WashOnly", "Regular", "Cancelled", "Paid", null, 2, 4, 22000, 0, 22000, "Demo: order dibatalkan sebelum diproses.");
        AddOrderDetail(order6, "Doll", "Boneka", "Item", null, 1, 0, 0, 22000, "Boneka hanya cuci saja.");

        int order7 = AddOrder("ORD-" + today.AddDays(-35).ToString("yyyyMMdd") + "-1007", angelina, today.AddDays(-35), today.AddDays(-33), "Regular", "WashAndIron", "Regular", "LatePickup", "Paid", null, 1, 3, 30000, 0, 30000, "Demo: belum diambil lebih dari 30 hari.");
        AddOrderDetail(order7, "BedCover", "Bed Cover", "Item", null, 1, 0, 0, 30000, "Contoh status terlambat.");
    }

    static int AddCustomer(string name, string phone, string address, string note)
    {
        return InsertAndGetId("INSERT INTO Customers(Name,Phone,Address,ImportantNote) VALUES(@p0,@p1,@p2,@p3)", name, phone, address, note);
    }

    static int AddCustomerPackage(int customerId, int planId, DateTime start, DateTime end, double initialQuota, double remainingQuota, string status)
    {
        return InsertAndGetId("INSERT INTO CustomerPackages(CustomerId,PackagePlanId,StartDate,EndDate,InitialQuotaKg,RemainingQuotaKg,Status) VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)",
            customerId, planId, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"), initialQuota, remainingQuota, status);
    }

    static void AddPackagePurchase(int customerId, int planId, int customerPackageId, DateTime createdAt, int amount)
    {
        Exec("INSERT INTO PackagePurchaseTransactions(CustomerId,PackagePlanId,CustomerPackageId,CreatedAt,Amount,PaymentStatus) VALUES(@p0,@p1,@p2,@p3,@p4,@p5)",
            customerId, planId, customerPackageId, createdAt.ToString("s"), amount, "Paid");
    }

    static int AddOrder(string orderNumber, int customerId, DateTime receivedAt, DateTime estimatedFinishAt, string processingMode, string serviceType, string serviceSpeed, string orderStatus, string paymentStatus, int? appliedPackageId, int receiverEmployeeId, int responsibleEmployeeId, int regularSubtotal, double packageCoveredWeight, int amountDueNow, string notes)
    {
        return InsertAndGetId(@"INSERT INTO LaundryOrders(OrderNumber,CustomerId,ReceivedAt,EstimatedFinishAt,ProcessingMode,ServiceType,ServiceSpeed,OrderStatus,PaymentStatus,AppliedPackageId,ReceiverEmployeeId,ResponsibleEmployeeId,RegularSubtotal,PackageCoveredWeight,AmountDueNow,Notes)
VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15)",
            orderNumber, customerId, receivedAt.ToString("s"), estimatedFinishAt.ToString("s"), processingMode, serviceType, serviceSpeed, orderStatus, paymentStatus,
            appliedPackageId.HasValue ? appliedPackageId.Value : DBNull.Value, receiverEmployeeId, responsibleEmployeeId, regularSubtotal, packageCoveredWeight, amountDueNow, notes);
    }

    static void AddOrderDetail(int orderId, string itemCategory, string itemName, string unitType, double? weightKg, int? quantity, int isPackageEligible, int isCoveredByPackage, int regularSubtotal, string notes)
    {
        Exec(@"INSERT INTO OrderDetails(OrderId,ItemCategory,ItemName,UnitType,WeightKg,Quantity,IsPackageEligible,IsCoveredByPackage,RegularSubtotal,Notes)
VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)",
            orderId, itemCategory, itemName, unitType,
            weightKg.HasValue ? weightKg.Value : DBNull.Value,
            quantity.HasValue ? quantity.Value : DBNull.Value,
            isPackageEligible, isCoveredByPackage, regularSubtotal, notes);
    }
}