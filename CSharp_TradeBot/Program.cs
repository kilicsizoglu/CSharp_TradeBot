using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_TradeBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var apikey = ApiKeyUtil.ReadApiKey();

            while (true)
            {
                var rsi = await TAApiConnect.GetStochRSI("REEFUSDT", apikey.TaApiApiKey);

                var orders = await BinanceApi.GetOrder(apikey.BinanceApiKey,
                                                        apikey.BinanceApiSecret,
                                                        "REEFUSDT");
                if (orders != null)
                {
                    foreach (var data in orders.Data)
                    {
                        if (data.Symbol == "REEFUSDT")
                        {
                            if (orders.Success)
                            {
                                CryptoOrder crypto = CryptoOrder.ReadFromJsonFile();
                                if (crypto != null)
                                {
                                    if (crypto.Side == "BUY")
                                    {
                                        if ((rsi.ValueFastK + rsi.ValueFastD / 2) <= 70)
                                        {
                                            if (rsi.ValueFastK < rsi.ValueFastD)
                                            {
                                                var order = await BinanceApi.Sell(apikey.BinanceApiKey,
                                                                             apikey.BinanceApiSecret,
                                                                             "REEFUSDT",
                                                                             data.Quantity);
                                                CryptoOrder.DeleteJsonFile();
                                            }
                                        }
                                    }
                                    else if (crypto.Side == "SELL")
                                    {
                                        if ((rsi.ValueFastK + rsi.ValueFastD / 2) <= 30)
                                        {
                                            if (rsi.ValueFastK > rsi.ValueFastD)
                                            {
                                                var order = await BinanceApi.Buy(apikey.BinanceApiKey,
                                                                             apikey.BinanceApiSecret,
                                                                             "REEFUSDT",
                                                                             data.Quantity);
                                                CryptoOrder.DeleteJsonFile();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                System.Console.WriteLine($"Buy Operation\n");
                if ((rsi.ValueFastK + rsi.ValueFastD) / 2 <= 30)
                {
                    if (rsi.ValueFastK > rsi.ValueFastD)
                    {
                        var price = await BinanceApi.GetPrice("REEFUSDT",
                                                              apikey.BinanceApiKey,
                                                              apikey.BinanceApiSecret);
                        var order = await BinanceApi.Buy(apikey.BinanceApiKey,
                                                         apikey.BinanceApiSecret,
                                                         "REEFUSDT",
                                                         66000);
                        if (order)
                        {
                            CryptoOrder crypto = new CryptoOrder();
                            crypto.Symbol = "REEFUSDT";
                            crypto.Side = "BUY";
                            CryptoOrder.WriteToJsonFile(crypto);
                            order = await BinanceApi.BuyLossProtect(apikey.BinanceApiKey,
                                                                    apikey.BinanceApiSecret,
                                                                    "REEFUSDT",
                                                                    66000,
                                                                    price,
                                                                    (decimal)((double)price - (((double)price / 100) * 1.5)));
                            System.Console.WriteLine($"Buy Ok\n");
                        }
                    }
                }
                System.Console.WriteLine($"Sell Operation\n");
                if ((rsi.ValueFastK + rsi.ValueFastD) / 2 <= 70)
                {
                    if (rsi.ValueFastK < rsi.ValueFastD)
                    {
                        var price = await BinanceApi.GetPrice("REEFUSDT",
                                                              apikey.BinanceApiKey,
                                                              apikey.BinanceApiSecret);
                        var order = await BinanceApi.Sell(apikey.BinanceApiKey,
                                                         apikey.BinanceApiSecret,
                                                         "REEFUSDT",
                                                         66000);
                        if (order)
                        {
                            CryptoOrder crypto = new CryptoOrder();
                            crypto.Symbol = "REEFUSDT";
                            crypto.Side = "SELL";
                            CryptoOrder.WriteToJsonFile(crypto);
                            order = await BinanceApi.SellLossProtect(apikey.BinanceApiKey,
                                                                     apikey.BinanceApiSecret,
                                                                     "REEFUSDT",
                                                                     66000,
                                                                     price,
                                                                     (decimal)((double)price + (((double)price / 100) * 1.5)));
                            System.Console.WriteLine($"Sell Ok\n");
                        }
                    }
                }
            }
        }
    }
}
