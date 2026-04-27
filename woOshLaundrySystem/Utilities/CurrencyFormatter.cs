using System.Globalization;
namespace woOshLaundrySystem.Utilities;
public static class CurrencyFormatter{ public static string Rupiah(int amount)=>string.Format(new CultureInfo("id-ID"),"{0:C0}",amount); }
