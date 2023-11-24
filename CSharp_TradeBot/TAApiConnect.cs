using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

public class StochRsi
{
    public double ValueFastK { get; set; }
    public double ValueFastD { get; set; }

    public StochRsi()
    {
        ValueFastK = 0;
        ValueFastD = 0;
    }
}

public static class TAApiConnect
{
    public static async Task<StochRsi> GetStochRSI(string coinName, string secret)
    {
        await Task.Delay(3000); 

        coinName = coinName == "ETHUSDT" ? "ETH/USDT" : coinName;
        string exchange = "binancefutures";
        string symbol = coinName;
        string interval = "4h";

        await Task.Delay(5000); // 5 saniye bekleme

        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://api.taapi.io/stochrsi?secret={secret}&exchange={exchange}&symbol={symbol}&interval={interval}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(content);

                var stochRsi = new StochRsi
                {
                    ValueFastK = data["valueFastK"].ToObject<double>(),
                    ValueFastD = data["valueFastD"].ToObject<double>()
                };

                if (stochRsi.ValueFastK == 0 && stochRsi.ValueFastD == 0)
                {
                    return new StochRsi();
                }
                else
                {
                    return stochRsi;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new StochRsi();
        }
    }
}
