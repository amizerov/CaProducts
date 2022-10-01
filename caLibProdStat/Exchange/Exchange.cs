using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public abstract class Exchange
{
    public abstract int ID { get; }
    public abstract string Name { get; }

    /// <summary>
    /// Main method of the service.
    /// Get historical data for product,
    /// analyse and store results to db for futher use
    /// </summary>
    public void ProcessProducts()
    {
        try
        {
            List<Product> products = GetProducts();
            foreach (var product in products)
            {
                List<Kline> klines;
                try
                {
                    klines = GetKlines(product.symbol, 1, 5);
                    
                    product.CalcStat(klines);
                    product.SaveStatToDb();

                    int Number = products.IndexOf(product);
                    Log.Trace(ID, $"ProcessProducts({product.symbol})",
                        $"{Number} of {products.Count} - [{product.TraceMessage}]");

                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Log.Error($"GetKlines({Name}/{product.symbol})", e.Message);
                }
            }
        }
        catch(Exception e)
        {
            Log.Error($"ProcessProducts({Name})", e.Message);
        }
    }

    /// <summary>
    /// Convert cpecific product for each exchange 
    /// to one model common class Product
    /// </summary>
    /// <param name="p">Object of type product for this exchange</param>
    /// <returns>Product of common type</returns>
    protected abstract Product ToProduct(Object p);

    /// <summary>
    /// Get list of all products
    /// </summary>
    /// <returns>List of all products for this exchange</returns>
    protected abstract List<Product> GetProducts();

    /// <summary>
    /// Get klines for symbol for a futher analysis
    /// </summary>
    /// <param name="symbol">Symbol of the product</param>
    /// <param name="IntervarInMinutes">Kline interval in minutes</param>
    /// <param name="PeriodInDays">Period for klines in days</param>
    /// <returns></returns>
    protected abstract List<Kline> GetKlines(string symbol, int IntervarInMinutes, int PeriodInDays);
}
