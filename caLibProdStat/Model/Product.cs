using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Product
{
    public int Id { get; set; }
    public string symbol { get; set; }
    public int exchange { get; set; }
    public string baseasset { get; set; }
    public string quoteasset { get; set; }
    public double volatility { get; set; }
    public double liquidity { get; set; }
    public int cnt1 { get; set; }
    public int cnt2 { get; set; }
    public int cnt3 { get; set; }
    public Product() { symbol = baseasset = quoteasset = ""; }

    public bool IsTradingEnabled;
    public void CalcStat(List<Kline> klines)
    {
        StatCalculator stat = new StatCalculator();
        try
        {
            stat.DoCalc(klines);
        }
        catch (Exception e)
        {
            Log.Error($"DoCalc({symbol})", e.Message);
        }
        volatility = stat.vola;
        liquidity = stat.liqu;

        cnt1 = stat.cnt1;
        cnt2 = stat.cnt2;
        cnt3 = stat.cnt3;
    }
    public string TraceMessage
    {
        get
        {
            return $"v={volatility};\tl={liquidity};\tc1={cnt1};\tc2={cnt2};\tc3={cnt3}";
        }
    }
    public void SaveStatToDb()
    {
        using (CaDb db = new())
        {
            try
            {
                Product? p = db.Products!.FirstOrDefault(p => p.symbol == symbol && p.exchange == exchange);
                if (p != null)
                {
                    db.Products!.Remove(p);
                }
                db.Products!.Add(this);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(exchange, $"DB SaveStatToDb({symbol}) Exception: ", ex.Message);
                if(ex.InnerException != null)
                    Log.Error(exchange, $"DB SaveStatToDb({symbol}) InnerException: ", ex.InnerException.Message);
            }
        }
    }
}
