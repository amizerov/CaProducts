using Huobi.Net.Enums;
using Huobi.Net.Clients;
using Huobi.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Huobi : AnExchange
{
    public override int ID => 3;
    public override string Name => "Huobi";

    HuobiClient client = new();

    protected override Product ToProduct(object p)
    {
        HuobiSymbol huobProd = (HuobiSymbol)p;

        Product product = new();
        product.symbol = huobProd.Name;
        product.exchange = ID;
        product.baseasset = huobProd.BaseAsset;
        product.quoteasset = huobProd.QuoteAsset;

        product.IsTradingEnabled = huobProd.State == SymbolState.Online;

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

        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, klInterval).Result; // !!! не хочет фром ту как в др биржах
                                                        //, DateTime.Now.AddDays(-1 * PeriodInDays), DateTime.Now).Result;

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
