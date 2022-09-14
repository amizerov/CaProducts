using CryptoExchange.Net;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public abstract class Exchange
{
    public abstract int ID { get; }
    public abstract string Name { get; }

    public void ProcessProducts()
    {
        List<Product> products = GetProducts();
        foreach (var product in products)
        {
            List<Kline> klines = GetKlines(product.symbol, 1, 5);

            product.CalcStat(klines);
            product.SaveStatToDb();

            Log.Trace(ID, $"ProcessProducts({product.symbol})", product.TraceMessage);
            Thread.Sleep(1000);
        }
    }
    protected abstract Product ToProduct(Object p);
    protected abstract List<Product> GetProducts();
    protected abstract List<Kline> GetKlines(string symbol, int IntervarInMinutes, int PeriodInDays);
}
