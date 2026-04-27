using woOshLaundrySystem.Data;
using woOshLaundrySystem.Enums;
using woOshLaundrySystem.Interfaces;
using woOshLaundrySystem.Models;
namespace woOshLaundrySystem.Services;
public class OrderService
{
    private readonly OrderRepository _orders=new(); private readonly PackageRepository _packages=new(); private readonly TariffCatalog _catalog=new();
    public OrderDetail CreateDetail(LaundryItemCategory cat,double weight,int qty,string notes){ LaundryItem item=cat switch{LaundryItemCategory.Clothing=>new ClothingItem(),LaundryItemCategory.Blanket=>new BlanketItem(),LaundryItemCategory.BedCover=>new BedCoverItem(),LaundryItemCategory.Carpet=>new CarpetItem(),_=>new DollItem()}; return new OrderDetail{Item=item,ItemCategory=item.Category,ItemName=item.ItemName,UnitType=item.UnitType,WeightKg=item.UnitType==UnitType.Kilogram?weight:null,Quantity=item.UnitType==UnitType.Item?qty:null,Notes=notes}; }
    public int CreateOrder(int customerId,List<OrderDetail> details,ServiceType service,ServiceSpeed speed,ProcessingMode mode,int receiverId,int workerId,string notes)
    {
        if(details.Count==0) throw new Exception("Item belum ditambahkan."); if(receiverId<=0||workerId<=0) throw new Exception("Karyawan penerima dan pekerja laundry wajib dipilih.");
        if(speed==ServiceSpeed.Express) mode=ProcessingMode.Regular;
        var order=new LaundryOrder{CustomerId=customerId,OrderNumber=$"ORD-{DateTime.Now:yyyyMMdd-HHmmss}",ServiceType=service,ServiceSpeed=speed,ProcessingMode=mode,PaymentStatus=woOshLaundrySystem.Enums.PaymentStatus.Paid,Notes=notes};
        order.Assignment.ReceiverEmployeeId=receiverId; order.Assignment.ResponsibleEmployeeId=workerId; order.Details=details;
        IProcessingPolicy policy=mode switch{ProcessingMode.PackageOnly=>new PackageOnlyProcessingPolicy(),ProcessingMode.Mixed=>new MixedProcessingPolicy(),_=>new RegularProcessingPolicy()};
        policy.Validate(order,_catalog);
        var allocation=policy.AllocatePackageCoverage(order); CustomerPackage? active=null;
        if(mode!=ProcessingMode.Regular){ active=_packages.GetActivePackage(customerId)??throw new Exception("Pelanggan tidak memiliki paket aktif."); if(allocation.CoveredWeight>active.RemainingQuotaKg) throw new Exception("Kuota paket tidak cukup."); order.AppliedPackageId=active.CustomerPackageId; }
        DateTime maxFinish=DateTime.Now;
        foreach(var d in order.Details){ var rule=_catalog.GetRule(d.ItemCategory,service,speed)??throw new Exception("Tarif tidak ditemukan."); IPriceCalculator calc=d.UnitType==UnitType.Kilogram?new MeasurementPriceCalculator():new CountedItemPriceCalculator(); maxFinish=new[]{maxFinish,calc.EstimateFinish(order.ReceivedAt,rule)}.Max(); if(mode!=ProcessingMode.Regular&&d.IsPackageEligible){d.CoverByPackage();} else {d.ChargeAsRegular(calc.CalculateRegularCharge(d,rule)); order.Pricing.AddRegularCharge(d.RegularSubtotal);} }
        order.Pricing.SetPackageCoverage(allocation.CoveredWeight); order.EstimatedFinishAt=maxFinish;
        int id=_orders.Add(order); if(active!=null&&allocation.CoveredWeight>0) _packages.ReduceQuota(active.CustomerPackageId,allocation.CoveredWeight); return id;
    }
    public List<LaundryOrder> GetAllOrders(string status="Semua")=>_orders.GetAll(status);
    public LaundryOrder? GetOrderDetail(int id)=>_orders.GetById(id);
    public void StartProcessing(int id){var o=_orders.GetById(id)??throw new Exception("Order tidak ditemukan.");o.StartProcessing();_orders.UpdateStatus(id,o.OrderStatus);}
    public void FinishOrder(int id){var o=_orders.GetById(id)??throw new Exception("Order tidak ditemukan.");o.Finish();_orders.UpdateStatus(id,o.OrderStatus);}
    public void PickUpOrder(int id){var o=_orders.GetById(id)??throw new Exception("Order tidak ditemukan.");o.PickUp();_orders.UpdateStatus(id,o.OrderStatus);}
    public void CancelOrder(int id){var o=_orders.GetById(id)??throw new Exception("Order tidak ditemukan.");o.Cancel();_orders.UpdateStatus(id,o.OrderStatus);}
}
