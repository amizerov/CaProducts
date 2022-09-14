using CryptoExchange.Net.CommonObjects;

namespace caLibProdStat
{
    public class StatCalculator
    {
        public double vola = 0;
        public double liqu = 0;
        public int cnt1 = 0;
        public int cnt2 = 0;
        public int cnt3 = 0;

        public void DoCalc(List<Kline> klines)
        {
            if (klines.Count == 0) return;

            int c1 = 0, c2 = 0, c3 = 0;
            double h, o, c, l, v, div = 0, vol = 0;

            double R = (double)klines.Average(k => k.ClosePrice)!;
            double H = (double)klines.Max(k => k.HighPrice)!;
            double L = (double)klines.Min(k => k.LowPrice)!;
            double V = (double)klines.Max(k => k.Volume)!;

            if (R * H * L * V == 0) return;

            foreach (Kline k in klines)
            {
                (h, o, c, l, v) =
                    ((double)k.HighPrice!, (double)k.OpenPrice!,
                     (double)k.ClosePrice!, (double)k.LowPrice!, (double)k.Volume!);

                div += Math.Pow((c - R) / R, 2);
                vol += v * h / (V * H);

                c1 += h == l ? 0 : 1;
                c2 += o == c ? 0 : 1;
                c3 += v == 0 ? 0 : 1;

            }
            cnt1 = 100 * c1 / klines.Count;
            cnt2 = 100 * c2 / klines.Count;
            cnt3 = 100 * c3 / klines.Count;

            vola = 100 * Math.Sqrt(div / klines.Count);
            liqu = 100 * vol / klines.Count;

            vola = Math.Round(vola, 2);
            liqu = Math.Round(liqu, 2);
        }
    }
}