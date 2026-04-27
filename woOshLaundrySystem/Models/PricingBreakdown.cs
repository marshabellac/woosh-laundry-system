namespace woOshLaundrySystem.Models;
public class PricingBreakdown
{
    public int RegularSubtotal { get; set; }
    public double PackageCoveredWeight { get; set; }
    public int AmountDueNow { get; set; }
    public void AddRegularCharge(int amount) { RegularSubtotal += amount; AmountDueNow = RegularSubtotal; }
    public void SetPackageCoverage(double weight) { PackageCoveredWeight = weight; }
}
