//using CryptoExchange.Net.Logging;
//TODO: а что это такое?

using amLogger;

namespace caLibProdStat
{
    public class ProductsUpdater
    {
        public static List<AnExchange> exchanges = new() {
                new Binance(),
                new Kucoin(),
                new Huobi()
            };
        
        public static void Start(Action? complete)
        {
            int cnt = 0;
            int ecnt = exchanges.Count;

            foreach (AnExchange ex in exchanges)
                Task.Run(() => {
                    Log.Info($"ProductsUpdater({ex.Name})", "Started");
                    ex.ProcessProducts();
                    Log.Info($"ProductsUpdater({ex.Name})", "Comleted");
                    cnt++;
                    Log.Info($"ProductsUpdater({ex.Name})", $"cnt = {cnt} of {ecnt}");
                    if (cnt == ecnt)
                    {
                        Log.Info($"ProductsUpdater({ex.Name})", "cnt == exs.Count");
                        if (complete != null)
                        {
                            complete();
                            Log.Info($"ProductsUpdater({ex.Name})", "complete is called!");

                        }
                    }
                });
        }
    }
}
