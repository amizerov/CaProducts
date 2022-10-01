using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Binance : AnExchange
{
    public override int ID => 1;
    public override string Name => "Binance";

    BinanceClient client = new();

    protected override Product ToProduct(object p)
    {
        BinanceProduct binaProd = (BinanceProduct)p;

        Product product = new();
        product.symbol = binaProd.Symbol;
        product.exchange = ID;
        product.baseasset = binaProd.BaseAsset;
        product.quoteasset = binaProd.QuoteAsset;

        product.IsTradingEnabled = binaProd.Status == "TRADING";

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        
        var r = client.SpotApi.ExchangeData.GetProductsAsync().Result;
        if (r.Success)
        {
            Log.Info(ID, "GetProducts", "start");
            foreach (var p in r.Data)
            {
                Product product = ToProduct(p);
                if (product.IsTradingEnabled) 
                { 
                    products.Add(product);
                }
            }
            Log.Info(ID, "GetProducts", "got " + products.Count);
        }
        return products;
    }
    protected override List<Kline> GetKlines(string symbol, int IntervarInMinutes, int PeriodInDays)
    {
        List<Kline> klines = new List<Kline>();

        int m = IntervarInMinutes % 60;
        int h = (IntervarInMinutes - m) / 60;
        TimeSpan klInterval = new TimeSpan(h, m, 0);
        
        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, klInterval, DateTime.Now.AddDays(-1 * PeriodInDays), DateTime.Now).Result;

        if (r.Success)
        {
            klines = r.Data.ToList();
        }
        else
        {
            Log.Error(ID, $"GetProductStat({symbol})", r.Error!.Message);
        }
        return klines;
    }
}
