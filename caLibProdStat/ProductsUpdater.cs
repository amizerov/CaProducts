namespace caLibProdStat
{
    public class ProductsUpdater
    {
        public static void Start(Action? complete)
        {
            List<Exchange> exs = new() {
                new Binance(),
                new Kucoin(),
                new Huobi()
            };
            int cnt = 0;
            foreach (Exchange ex in exs)
                Task.Run(() => {
                    ex.ProcessProducts();
                    cnt++;
                    if (cnt == exs.Count) 
                        if(complete != null) complete();
                });
        }
    }
}
