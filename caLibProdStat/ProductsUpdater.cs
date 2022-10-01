//using CryptoExchange.Net.Logging;
using amLogger;

namespace caLibProdStat
{
    public class ProductsUpdater
    {
        public static void Start(Action? complete)
        {
            List<AnExchange> exs = new() {
                new Binance(),
                new Kucoin(),
                new Huobi()
            };
            int cnt = 0;
            foreach (AnExchange ex in exs)
                Task.Run(() => {
                    ex.ProcessProducts();
                    Log.Info($"ProductsUpdater({ex.Name})", "Comleted");
                    cnt++;
                    Log.Info($"ProductsUpdater({ex.Name})", $"cnt = {cnt} of {exs.Count}");
                    if (cnt == exs.Count)
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
