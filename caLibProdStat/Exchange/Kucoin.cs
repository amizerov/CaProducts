using Kucoin.Net.Clients;
using Kucoin.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Kucoin : Exchange
{
    public override int ID => 2;
    public override string Name => "Kucoin";

    KucoinClient client = new();

    protected override Product ToProduct(object p)
    {
        KucoinSymbol kucoProd = (KucoinSymbol)p;

        Product product = new();
        product.symbol = kucoProd.Symbol;
        product.exchange = ID;
        product.baseasset = kucoProd.BaseAsset;
        product.quoteasset = kucoProd.QuoteAsset;

        product.IsTradingEnabled = kucoProd.EnableTrading;

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        
        var r = client.SpotApi.ExchangeData.GetSymbolsAsync().Result;
        if (r.Success)
        {
            Log.Trace(ID, "GetProducts", "start");
            foreach (var p in r.Data)
            {
                Product product = ToProduct(p);
                if (product.IsTradingEnabled)
                {
                    products.Add(product);
                }
            }
            Log.Trace(ID, "GetProducts", "got " + products.Count);
        }
        return products;
    }
    protected override List<Kline> GetKlines(string symbol, int IntervarInMinutes, int PeriodInDays)
    {
        List<Kline> klines = new List<Kline>();

        int m = IntervarInMinutes % 60;
        int h = (IntervarInMinutes - m) / 60;
        TimeSpan klInterval = new TimeSpan(h, m, 0);

        int countTrys = 3;
        while (--countTrys > 0)
        {
            var r = client.SpotApi.CommonSpotClient
                .GetKlinesAsync(symbol, klInterval, DateTime.Now.AddDays(-1 * PeriodInDays), DateTime.Now).Result;

            if (r.Success)
            {
                klines = r.Data.ToList();
                break;
            }
            else
            {
                string err = r.Error!.Message;
                Log.Error(ID, $"GetProductStat({symbol})", r.Error!.Message);

                if (err.Contains("Too Many"))
                {
                    Thread.Sleep(3000);
                }
                else
                    break;
            }
        }
        return klines;
    }
}
